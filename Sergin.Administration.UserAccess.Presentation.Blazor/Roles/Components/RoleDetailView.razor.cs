using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.SharedKernel.Presentation.Blazor.Modules;
using Sergin.SharedKernel.Presentation.Errors;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Delete;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GrantPermission;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Rename;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.RevokePermission;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Models;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Components;

/// <summary>
/// One role: its code, owner and inheritability, rename, its permissions, and delete. Every edit is sent at the version
/// the view loaded.
/// </summary>
/// <remarks>
/// The role permission is checked by the handlers against the role's owner scope, so Delete is hidden for a user who
/// holds it on no scope at all and refused by the handler otherwise.
/// </remarks>
public sealed partial class RoleDetailView
{
    private readonly RoleNameFormModel model = new();

    private RoleQueryResponse? role;
    private RowVersion? version;
    private SerginProblem? problem;
    private string? permissionToGrant;
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
    private PermissionCatalog Catalog { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    // The modules this host shows, whose drawer group titles name the permission picker's module headings.
    [Inject]
    private SerginUiModuleCatalog Modules { get; set; } = default!;

    [CascadingParameter]
    private SerginViewHost Host { get; set; } = SerginViewHost.Unhosted;

    private EventCallback OnRename => EventCallback.Factory.Create(this, RenameAsync);

    private IReadOnlyList<SerginBreadcrumb> Trail =>
    [
        SerginBreadcrumb.Of(UserAccessNavigation.Scopes),
        role is null ? new(ScopeTexts.Singular) : SerginBreadcrumb.Data(role.OwnerScopeName, $"/ua/scopes/{role.OwnerScopeId}"),
        role is null ? new(RoleTexts.Singular) : SerginBreadcrumb.Data(role.Name),
    ];

    private IReadOnlyList<SerginPageAction> Actions =>
    [
        new(CommonTexts.Delete, Icons.Material.Outlined.Delete, SerginActionKind.Danger, OnClick: EventCallback.Factory.Create(this, DeleteAsync),
            Permission: RolePermissions.Manage, AnyScope: true, Disabled: busy),
    ];

    // The role's grants the catalogue defines, shown by label; a grant it no longer defines is listed apart by its
    // raw name, still revocable.
    private IEnumerable<PermissionDefinition> GrantedDefined =>
        (role?.Permissions ?? []).Select(TryDefine).OfType<PermissionDefinition>();

    private IReadOnlyCollection<string> GrantedUnknown =>
        [.. (role?.Permissions ?? []).Where(permission => TryDefine(permission) is null)];

    protected override void OnInitialized() => validation = FormValidator.RulesFor(ToRenameCommand);

    // The pickers' module headings, one per drawer group, so modules sharing a group show once.
    private IReadOnlyList<PermissionPickerSection> PickerSections => PermissionLabels.Sections(Catalog, Modules, Localizer);

    private string LabelOf(string? name) => name is null ? string.Empty : PermissionLabels.Of(Catalog, Localizer, name);

    // A stored row that is not even a well-formed name is unknown too, not a render failure.
    private PermissionDefinition? TryDefine(string permission) =>
        Permission.TryCreate(permission, out Permission? parsed) && Catalog.TryGet(parsed, out PermissionDefinition? definition)
            ? definition
            : null;

    private bool IsGranted(PermissionDefinition definition) =>
        role?.Permissions.Contains(definition.Permission.Value) == true;

    protected override Task OnParametersSetAsync() => LoadAsync();

    private async Task LoadAsync()
    {
        ErrorOr<Versioned<RoleQueryResponse>> loaded = await Dispatcher.SendVersionedAsync(new GetRoleByIdQueryCommand(Id));

        if (loaded.IsError)
        {
            role = null;
            version = null;
            problem = ErrorPresenter.Present(loaded.FirstError);

            return;
        }

        problem = null;
        role = loaded.Value.Value;
        version = loaded.Value.Version;
        model.Name = role.Name;
    }

    private RenameRoleCommand ToRenameCommand() => new(Id, new RoleName(model.Name));

    private async Task RenameAsync()
    {
        await form.ValidateAsync();

        if (form.IsValid)
        {
            await SendGuardedAsync(ToRenameCommand());
        }
    }

    private Task SetInheritableAsync(bool isInheritable) => SendGuardedAsync(new SetRoleInheritableCommand(Id, isInheritable));

    private Task GrantAsync() =>
        permissionToGrant is null ? Task.CompletedTask : SendGuardedAsync(new GrantRolePermissionCommand(Id, Permission.Create(permissionToGrant)));

    private Task RevokeAsync(string permission) => SendGuardedAsync(new RevokeRolePermissionCommand(Id, Permission.Create(permission)));

    // Every edit is sent at the version this view loaded; the view reloads afterwards either way, so a stale
    // answer leaves it showing the current role.
    private async Task SendGuardedAsync<TResponse>(IRequest<ErrorOr<TResponse>> command)
    {
        if (version is not { } expected)
        {
            return;
        }

        busy = true;
        ErrorOr<Versioned<TResponse>> result = await Dispatcher.SendVersionedAsync(command, expected);
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);
        }

        permissionToGrant = null;
        await LoadAsync();
    }

    private async Task DeleteAsync()
    {
        if (version is not { } expected || role is null)
        {
            return;
        }

        if (!await DialogService.ConfirmDeleteAsync(
                Localizer, RoleTexts.DeleteTitle, cascade ? RoleTexts.DeleteCascadeConfirm : RoleTexts.DeleteConfirm, new { role.Name }))
        {
            return;
        }

        Guid ownerScopeId = role.OwnerScopeId;

        busy = true;
        ErrorOr<Versioned<Deleted>> result = await Dispatcher.SendVersionedAsync(new DeleteRoleCommand(Id, cascade), expected);
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);
            await LoadAsync();

            return;
        }

        await Host.LeaveAsync($"/ua/scopes/{ownerScopeId}");
    }
}
