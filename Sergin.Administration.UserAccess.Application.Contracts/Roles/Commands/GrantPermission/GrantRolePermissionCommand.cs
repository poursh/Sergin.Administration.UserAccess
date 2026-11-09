using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GrantPermission;

public sealed record GrantRolePermissionCommand(RoleId Id, Permission Permission)
    : ICommand<GrantRolePermissionCommandResponse>, IAddValueCommand<Role, RoleId, Permission>;
