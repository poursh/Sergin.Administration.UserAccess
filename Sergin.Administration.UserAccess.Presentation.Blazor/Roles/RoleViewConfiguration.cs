using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Create;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Delete;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Rename;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Components;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates.Configuration;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates.Model;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles;

/// <summary>
/// The role's generated views, drawn inside the scope tree's pane under the owner scope (<c>/ua/scopes/{scopeId}/roles/…</c>):
/// it belongs to its owner scope (trail, link and where a delete leaves to), rename and inheritability are edited inline,
/// a delete asks whether to revoke the assignments too, a new role takes its owner scope from the route and its
/// permissions from the picker, and the granted permissions are drawn by their own slot.
/// </summary>
internal sealed class RoleViewConfiguration : IAggregateViewConfiguration<Role>
{
    public void Configure(AggregateViewBuilder<Role> view)
    {
        // Under the owner scope, so both pages open in the scope tree's layout with the owner's node selected.
        view.DetailUnderParent((scopeId, id) => $"/ua/scopes/{scopeId}/roles/{id}");
        view.BelongsTo<Scope, RoleQueryResponse>(role => role.OwnerScopeId, role => role.OwnerScopeName);

        view.Command<CreateRoleCommand>().Label(RoleUiTexts.New);
        view.Command<RenameRoleCommand>().Label(CommonTexts.Rename).Edit(SerginEditMode.Inline);
        view.Command<SetRoleInheritableCommand>().Edit(SerginEditMode.Inline);
        view.Command<DeleteRoleCommand>().ConfirmDelete(RoleUiTexts.DeleteTitle, RoleUiTexts.DeleteConfirm);

        view.Detail<RoleQueryResponse>(detail =>
        {
            detail.Title(x => x.Name);
            detail.Field(x => x.OwnerScopeName).Label(RoleUiTexts.OwnedBy).Link(role => $"/ua/scopes/{role.OwnerScopeId}");
            detail.Field(x => x.Permissions).Hidden();
        });
        view.Form<CreateRoleCommand>(form =>
        {
            form.Title(RoleUiTexts.New).Parent(x => x.OwnerScopeId);
            form.Field(x => x.IsInheritable).Label(RoleUiTexts.InheritableOnCreate);
            form.Field(x => x.Permissions).Component<PermissionPickerInput>();
        });

        view.Slot<RolePermissionsSlot>(SerginViewSlot.DetailEntities);
    }
}
