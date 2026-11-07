using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Create;

public sealed record CreateRoleCommand(
    Guid OwnerScopeId, RoleName Name, bool IsInheritable, IReadOnlyCollection<Permission> Permissions)
    : ICommand<CreateRoleCommandResponse>;
