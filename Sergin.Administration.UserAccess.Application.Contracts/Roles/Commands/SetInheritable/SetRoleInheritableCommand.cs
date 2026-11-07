using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;

public sealed record SetRoleInheritableCommand(Guid Id, bool IsInheritable) : ICommand<SetRoleInheritableCommandResponse>;
