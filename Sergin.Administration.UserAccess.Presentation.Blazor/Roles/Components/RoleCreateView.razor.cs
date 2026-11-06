using Microsoft.AspNetCore.Components;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.SharedKernel.Presentation.Blazor.Modules;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Create;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Models;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Components;

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
    private IStringLocalizer Localizer { get; set; } = default!;

    // The modules this host shows, whose drawer group titles name the permission picker's module headings.
    [Inject]
    private SerginUiModuleCatalog Modules { get; set; } = default!;

    [CascadingParameter]
    private SerginViewHost Host { get; set; } = SerginViewHost.Unhosted;

    // Created on this view, so it re-renders after SubmitAsync; SerginFormView invokes it once every field is valid.
    private EventCallback OnSubmit => EventCallback.Factory.Create(this, SubmitAsync);

    private IReadOnlyList<SerginBreadcrumb> Trail =>
    [
        SerginBreadcrumb.Of(UserAccessNavigation.Scopes),
        new(ScopeTexts.Singular, $"/ua/scopes/{ScopeId}"),
        new(RoleTexts.New),
    ];

    // The pickers' module headings, one per drawer group, so modules sharing a group show once.
    private IReadOnlyList<PermissionPickerSection> PickerSections => PermissionLabels.Sections(Catalog, Modules, Localizer);

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
