using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetList;
using Sergin.Administration.UserAccess.Application.Configurations.Users;

namespace Sergin.Administration.UserAccess.Application.Users.Assignments.Commands.GetList;

internal sealed class GetUserAssignmentsQueryCommandHandler(IUserAssignmentQueryRepository repository, UserReachGuard reach)
    : IQueryHandler<GetUserAssignmentsQueryCommand, IReadOnlyCollection<UserAssignmentItem>>
{
    public async Task<ErrorOr<IReadOnlyCollection<UserAssignmentItem>>> Handle(
        GetUserAssignmentsQueryCommand request, CancellationToken cancellationToken)
    {
        // A regional reader does not see the grants of someone granted outside their region.
        if (!await reach.CoversAsync(request.RootId.Value, UserPermissions.Read, cancellationToken))
        {
            return Error.Forbidden();
        }

        return (await repository.GetUserAssignmentsAsync(request.RootId.Value, cancellationToken)).ToErrorOr();
    }
}
