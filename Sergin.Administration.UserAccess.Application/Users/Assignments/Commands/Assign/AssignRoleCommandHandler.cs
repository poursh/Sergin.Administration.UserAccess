using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Assign;
using Sergin.SharedKernel.Application.Times;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;
using Sergin.Administration.UserAccess.Application.Configurations.Users;

namespace Sergin.Administration.UserAccess.Application.Users.Assignments.Commands.Assign;

internal sealed class AssignRoleCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IUserRepository users,
    IRoleRepository roles,
    IScopeRepository scopes,
    IScopeAuthorization authorization,
    IDateTimeProvider clock,
    IAuthorizationCacheInvalidator cache) : ICommandHandler<AssignRoleCommand, AssignRoleCommandResponse>
{
    public async Task<ErrorOr<AssignRoleCommandResponse>> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        if (!await authorization.CurrentUserHasAsync(UserPermissions.AssignmentsManage, request.ScopeId, cancellationToken))
        {
            return Error.Forbidden();
        }

        User? user = await users.GetWithAssignmentsAsync(request.RootId, cancellationToken);
        Role? role = await roles.GetAsync(new RoleId(request.RoleId), cancellationToken);
        Scope? scope = await scopes.GetAsync(new ScopeId(request.ScopeId), cancellationToken);

        if (user is null || role is null || scope is null)
        {
            return Error.NotFound();
        }

        Scope? owner = await scopes.GetAsync(role.OwnerScopeId, cancellationToken);

        if (owner is null)
        {
            return Error.NotFound();
        }

        ErrorOr<RoleAssignment> assigned = ValidityPeriod.Create(request.ValidFromUtc, request.ValidUntilUtc)
            .Then(period => user.AssignRole(role, owner.Path, scope, request.IncludeSubtree, period, clock.UtcNow));

        if (assigned.IsError)
        {
            return assigned.Errors;
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cache.Invalidate();

        return new AssignRoleCommandResponse(assigned.Value.Id.Value);
    }
}
