using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.DeactivateUser;

public sealed record DeactivateUserCommand(UserInternalId Id) : ICommand<DeactivateUserCommandResponse>, ITransitionCommand<User, UserInternalId>;
