using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.Create;

public sealed record CreateUserCommand(UserName UserName) : ICommand<CreateUserCommandResponse>, ICreateCommand<User, UserInternalId>;
