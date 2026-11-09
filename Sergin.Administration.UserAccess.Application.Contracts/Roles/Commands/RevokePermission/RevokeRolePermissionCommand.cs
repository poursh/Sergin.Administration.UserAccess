using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.RevokePermission;

public sealed record RevokeRolePermissionCommand(RoleId Id, Permission Permission)
    : ICommand<RevokeRolePermissionCommandResponse>, IRemoveValueCommand<Role, RoleId, Permission>;
