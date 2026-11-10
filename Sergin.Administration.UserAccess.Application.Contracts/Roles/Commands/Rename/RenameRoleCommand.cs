using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Rename;

public sealed record RenameRoleCommand(RoleId Id, RoleName Name) : ICommand<RenameRoleCommandResponse>, ISetPropertyCommand<Role, RoleId>;
