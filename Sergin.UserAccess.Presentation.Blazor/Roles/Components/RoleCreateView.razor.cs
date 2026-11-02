using Microsoft.AspNetCore.Components;
using Sergin.SharedKernel.Application.Localizations;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.UserAccess.Application.Contracts.Roles.Commands.Create;
using Sergin.UserAccess.Domain.Roles;
using Sergin.UserAccess.Presentation.Blazor.Roles.Models;

namespace Sergin.UserAccess.Presentation.Blazor.Roles.Components;

/// <summary>The form for a new role owned by <see cref="ScopeId"/>; saving opens the new role.</summary>
public sealed partial class RoleCreateView
{
    private readonly NewRoleFormModel model = new();

    private bool submitting;
    private Func<object, string, Task<IEnumerable<string>>> validation = default!;

    /// <summary>The scope that owns the new role.</summary>
    [Parameter]
    [EditorRequired]
    public Guid ScopeId { get; set; }

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private ISerginFormValidator FormValidator { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    // The permission catalogue: every permission an aggregate registered. The server still refuses one the
    // user does not hold in this scope (the permission ceiling).
    [Inject]
    private PermissionCatalog Catalog { get; set; } = default!;

    [Inject]
    private ILocalizer Localizer { get; set; } = default!;

    [CascadingParameter]
    private SerginViewHost Host { get; set; } = SerginViewHost.Unhosted;

    // Created on this view, so it re-renders after SubmitAsync; SerginFormView invokes it once every field is valid.
    private EventCallback OnSubmit => EventCallback.Factory.Create(this, SubmitAsync);

    private IReadOnlyList<SerginBreadcrumb> Trail =>
    [
        SerginBreadcrumb.Of(UserAccessNavigation.Scopes),
        new("Scope", $"/ua/scopes/{ScopeId}"),
        new("New role"),
    ];

    private string LabelOf(string? name) => name is null ? string.Empty : PermissionLabels.Of(Catalog, Localizer, name);

    private string SelectedText(IReadOnlyList<string> selected) =>
        string.Join(", ", selected.Select(LabelOf));

    protected override void OnInitialized() => validation = FormValidator.RulesFor(ToCommand);

    // One mapping for both the field-by-field validation and the submit, so the two cannot drift.
    private CreateRoleCommand ToCommand() => new(
        ScopeId,
        new RoleName(model.Name),
        model.IsInheritable,
        [.. model.Permissions.Select(Permission.Create)]);

    private async Task SubmitAsync()
    {
        submitting = true;

        ErrorOr<CreateRoleCommandResponse> result = await Dispatcher.SendAsync(ToCommand());

        submitting = false;

        if (result.IsError)
        {
            // Every error, not the first: validation yields one per broken rule.
            ErrorPresenter.Notify(result.Errors);

            return;
        }

        await Host.LeaveAsync($"/ua/roles/{result.Value.Id}");
    }
}
