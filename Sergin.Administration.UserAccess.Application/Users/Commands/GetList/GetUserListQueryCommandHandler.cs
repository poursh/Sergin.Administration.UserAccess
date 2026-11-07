using ErrorOr;
using Sergin.SharedKernel.Application;
using Sergin.SharedKernel.Application.Commands.Queries;

namespace Sergin.Administration.UserAccess.Application.Users.Commands.GetList;

internal sealed class GetUserListQueryCommandHandler(IGetUserListQueryRepository queryRepository) : IListQueryHandler<GetUserListQueryCommand, GetUserListItem>
{
    public async Task<ErrorOr<ListQueryResponse<GetUserListItem>>> Handle(
        GetUserListQueryCommand request, CancellationToken cancellationToken)
    {
        ListQueryResponse<GetUserListItem> res = await queryRepository.GetListAsync(request, cancellationToken);

        return res;
    }
}
