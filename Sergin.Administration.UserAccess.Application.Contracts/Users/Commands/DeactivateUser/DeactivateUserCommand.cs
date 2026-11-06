using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.DeactivateUser;

public sealed record DeactivateUserCommand(Guid Id) : ICommand<DeactivateUserCommandResponse>;
