using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.UserAccess.Application.Users.Commands.GetList;
public interface IGetUserListQueryRepository
{
    Task<ListQueryResponse<GetUserListItem>> GetListAsync(ListQuery query, CancellationToken cancellationToken = default);
}
