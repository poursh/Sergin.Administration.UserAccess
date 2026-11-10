using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Delete;
using Sergin.SharedKernel.Application.Securities.Users;
using Sergin.SharedKernel.Application.Times;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Delete;

internal sealed class DeleteScopeCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IScopeRepository scopes,
    IRoleRepository roles,
    IUserRepository users,
    IScopeAuthorization authorization,
    IUserContext userContext,
    IDateTimeProvider clock,
    IAuthorizationCacheInvalidator cache) : ICommandHandler<DeleteScopeCommand, Deleted>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteScopeCommand request, CancellationToken cancellationToken)
    {
        if (!await authorization.CurrentUserHasAsync(ScopePermissions.Manage, request.Id.Value, cancellationToken))
        {
            return Error.Forbidden();
        }

        Scope? scope = await scopes.GetAsync(request.Id, cancellationToken);

        if (scope is null)
        {
            return Error.NotFound();
        }

        // The validator refused a non-cascading delete of a scope still in use, so here either nothing is in use
        // or the caller chose the cascade (BR-5).
        if (request.Cascade)
        {
            foreach (User user in await users.GetWithLiveAssignmentsAtScopeAsync(request.Id.Value, cancellationToken))
            {
                foreach (RoleAssignment assignment in user.Assignments.Where(a => a.ScopeId == scope.Id && !a.IsRevoked).ToList())
                {
                    user.RevokeAssignment(assignment.Id, clock.UtcNow, userContext.Id.Value);
                }
            }

            foreach (Role role in await roles.GetOwnedByAsync(scope.Id, cancellationToken))
            {
                roles.Remove(role);
            }
        }

        scopes.Remove(scope);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cache.Invalidate();

        return Result.Deleted;
    }
}
