using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;

public sealed record SetRoleInheritableCommand(RoleId Id, bool IsInheritable)
    : ICommand<SetRoleInheritableCommandResponse>, ISetPropertyCommand<Role, RoleId>;
