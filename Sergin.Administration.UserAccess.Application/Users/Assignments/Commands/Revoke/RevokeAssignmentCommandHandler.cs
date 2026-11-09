using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Revoke;
using Sergin.SharedKernel.Application.Securities.Users;
using Sergin.SharedKernel.Application.Times;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;
using Sergin.Administration.UserAccess.Application.Configurations.Users;

namespace Sergin.Administration.UserAccess.Application.Users.Assignments.Commands.Revoke;

internal sealed class RevokeAssignmentCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IUserRepository users,
    IScopeAuthorization authorization,
    IUserContext userContext,
    IDateTimeProvider clock,
    IAuthorizationCacheInvalidator cache) : ICommandHandler<RevokeAssignmentCommand, RevokeAssignmentCommandResponse>
{
    public async Task<ErrorOr<RevokeAssignmentCommandResponse>> Handle(RevokeAssignmentCommand request, CancellationToken cancellationToken)
    {
        User? user = await users.GetWithAssignmentsAsync(request.RootId, cancellationToken);
        RoleAssignment? assignment = user?.Assignments.FirstOrDefault(a => a.Id == request.Id);

        if (user is null || assignment is null)
        {
            return Error.NotFound();
        }

        if (!await authorization.CurrentUserHasAsync(UserPermissions.AssignmentsManage, assignment.ScopeId.Value, cancellationToken))
        {
            return Error.Forbidden();
        }

        ErrorOr<Success> revoked = user.RevokeAssignment(request.Id, clock.UtcNow, userContext.Id.Value);

        if (revoked.IsError)
        {
            return revoked.Errors;
        }

        // FR-12: effective immediately — the cache is dropped as soon as the revocation is committed.
        await unitOfWork.SaveChangesAsync(cancellationToken);
        cache.Invalidate();

        return new RevokeAssignmentCommandResponse(request.Id.Value);
    }
}
