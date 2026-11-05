using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.SharedKernel.Presentation.Errors;
using Sergin.UserAccess.Application.Configurations.Scopes;
using Sergin.UserAccess.Application.Contracts.Roles.Commands.GetAssignable;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Delete;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.GetOne;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.GetTree;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Move;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Rename;
using Sergin.UserAccess.Domain.Scopes;
using Sergin.UserAccess.Presentation.Blazor.Scopes.Models;

namespace Sergin.UserAccess.Presentation.Blazor.Scopes.Components;

/// <summary>
/// One scope: its name and parent, rename and move, its child scopes and the roles usable at it. Hosted by
/// <c>ScopeDetailPage</c> as a page and by <c>ScopeTreePage</c> as the tree's selected content.
/// </summary>
/// <remarks>
/// The scope permission is checked by the handlers against the target scope, so the actions here are hidden for a user
/// who holds it on no scope at all and refused by the handler otherwise. After a rename or a move the view tells its
/// host, so the tree beside it shows the new name or place.
/// </remarks>
public sealed partial class ScopeDetailView
{
    private readonly ScopeNameFormModel model = new();

    private ScopeQueryResponse? scope;
    private RowVersion? version;
    private SerginProblem? problem;
    private IReadOnlyCollection<AssignableRoleItem> roles = [];
    private IReadOnlyCollection<ScopeTreeItem> tree = [];
    private IReadOnlyCollection<ScopeTreeItem> moveTargets = [];
    private Guid newParentId;
    private bool cascade;
    private bool busy;

    private MudForm form = default!;
    private bool isValid;
    private Func<object, string, Task<IEnumerable<string>>> validation = default!;

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private ISerginFormValidator FormValidator { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [CascadingParameter]
    private SerginViewHost Host { get; set; } = SerginViewHost.Unhosted;

    private EventCallback OnRename => EventCallback.Factory.Create(this, RenameAsync);

    private IReadOnlyList<SerginBreadcrumb> Trail =>
    [
        SerginBreadcrumb.Of(UserAccessNavigation.Scopes),
        scope is null ? new(ScopeTexts.Singular) : SerginBreadcrumb.Data(scope.Name),
    ];

    // The root cannot be deleted; every other scope can, once it is a leaf (the handler refuses otherwise).
    private IReadOnlyList<SerginPageAction> Actions =>
    [
        new(ScopeTexts.NewChild, Icons.Material.Outlined.Add, SerginActionKind.Secondary, Href: $"/ua/scopes/new?parentId={Id}",
            Permission: ScopePermissions.Manage, AnyScope: true),
        new(ScopeTexts.NewRoleHere, Icons.Material.Outlined.Badge, SerginActionKind.Secondary, Href: $"/ua/roles/new?scopeId={Id}"),
        .. scope?.ParentId is null
            ? Array.Empty<SerginPageAction>()
            : [new(CommonTexts.Delete, Icons.Material.Outlined.Delete, SerginActionKind.Danger, OnClick: EventCallback.Factory.Create(this, DeleteAsync),
                Permission: ScopePermissions.Manage, AnyScope: true, Disabled: busy)],
    ];

    protected override void OnInitialized() => validation = FormValidator.RulesFor(ToRenameCommand);

    protected override Task OnParametersSetAsync() => LoadAsync();

    private async Task LoadAsync()
    {
        ErrorOr<Versioned<ScopeQueryResponse>> loaded = await Dispatcher.SendVersionedAsync(new GetScopeByIdQueryCommand(Id));

        if (loaded.IsError)
        {
            scope = null;
            version = null;
            problem = ErrorPresenter.Present(loaded.FirstError);

            return;
        }

        problem = null;
        scope = loaded.Value.Value;
        version = loaded.Value.Version;
        model.Name = scope.Name;

        ErrorOr<IReadOnlyCollection<AssignableRoleItem>> assignable = await Dispatcher.SendAsync(new GetAssignableRolesQueryCommand(Id));
        roles = assignable.IsError ? [] : assignable.Value;

        // Anywhere but here and below: the server refuses a cycle too, this just keeps it out of the list.
        ErrorOr<IReadOnlyCollection<ScopeTreeItem>> loadedTree = await Dispatcher.SendAsync(new GetScopeTreeQueryCommand());
        tree = loadedTree.IsError ? [] : loadedTree.Value;
        moveTargets = [.. tree.Where(candidate => !IsSelfOrBelow(candidate))];
    }

    private bool IsSelfOrBelow(ScopeTreeItem candidate)
    {
        ScopeTreeItem? current = candidate;

        while (current is not null)
        {
            if (current.Id == Id)
            {
                return true;
            }

            current = tree.FirstOrDefault(item => item.Id == current.ParentId);
        }

        return false;
    }

    private RenameScopeCommand ToRenameCommand() => new(Id, new ScopeName(model.Name));

    private async Task RenameAsync()
    {
        await form.ValidateAsync();

        if (!form.IsValid || version is not { } expected)
        {
            return;
        }

        busy = true;
        ErrorOr<Versioned<RenameScopeCommandResponse>> result = await Dispatcher.SendVersionedAsync(ToRenameCommand(), expected);
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);
        }

        await LoadAsync();

        if (!result.IsError)
        {
            await Host.ChangedAsync();
        }
    }

    private async Task MoveAsync()
    {
        busy = true;
        ErrorOr<MoveScopeCommandResponse> result = await Dispatcher.SendAsync(new MoveScopeCommand(Id, newParentId));
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);

            return;
        }

        await LoadAsync();
        await Host.ChangedAsync();
    }

    private async Task DeleteAsync()
    {
        if (!await DialogService.ConfirmDeleteAsync(Localizer, ScopeTexts.DeleteTitle, ScopeTexts.DeleteConfirm, new { scope?.Name }))
        {
            return;
        }

        busy = true;
        ErrorOr<Deleted> result = await Dispatcher.SendAsync(new DeleteScopeCommand(Id, cascade));
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);

            return;
        }

        await Host.LeaveAsync(scope?.ParentId is { } parentId ? $"/ua/scopes/{parentId}" : "/ua/scopes");
    }
}
