using Sergin.Administration.UserAccess.Application.Users.Commands.GetList;
using Sergin.Administration.UserAccess.Application.Users.Commands.GetOne;

namespace Sergin.Administration.UserAccess.Application.Users;
public interface IUserAllQueryRepository
    : IGetUserListQueryRepository, IGetUserQueryRepository;
