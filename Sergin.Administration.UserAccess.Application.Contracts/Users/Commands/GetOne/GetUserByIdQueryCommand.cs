using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.GetOne;

public sealed record GetUserByIdQueryCommand(UserInternalId Id) : IQuery<UserQueryResponse>, IGetOneQuery<User, UserInternalId>;
