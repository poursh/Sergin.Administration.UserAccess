using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Create;

/// <remarks><paramref name="OwnerScopeId"/> is a plain reference, not a structural parent: a role is not a node of the scope tree.</remarks>
public sealed record CreateRoleCommand(
    Guid OwnerScopeId, RoleName Name, bool IsInheritable, IReadOnlyCollection<Permission> Permissions)
    : ICommand<CreateRoleCommandResponse>, ICreateCommand<Role, RoleId>;
