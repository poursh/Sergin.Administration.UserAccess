using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.GetList;

public sealed record GetUserListQueryCommand : ListQuery<GetUserListItem>, IListOf<User>
{
    public GetUserListQueryCommand(
        Paggination paggination,
        Term? term = default,
        Filtering? filtering = default,
        Sorting? sorting = default)
        : base(paggination, term, filtering, sorting)
    {
    }
}
