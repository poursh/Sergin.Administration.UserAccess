using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Users.Commands.GetOne;

public interface IGetUserQueryRepository
{
    Task<UserQueryResponse?> GetUserById(UserInternalId Id, CancellationToken cancellationToken = default);
}
