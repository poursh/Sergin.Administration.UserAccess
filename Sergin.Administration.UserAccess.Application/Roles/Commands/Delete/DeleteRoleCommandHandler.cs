using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Delete;
using Sergin.SharedKernel.Application.Securities.Users;
using Sergin.SharedKernel.Application.Times;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;

namespace Sergin.Administration.UserAccess.Application.Roles.Commands.Delete;

internal sealed class DeleteRoleCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IRoleRepository roles,
    IUserRepository users,
    IScopeAuthorization authorization,
    IUserContext userContext,
    IDateTimeProvider clock,
    IAuthorizationCacheInvalidator cache) : ICommandHandler<DeleteRoleCommand, Deleted>
{
    public async Task<ErrorOr<Deleted>> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        Role? role = await roles.GetAsync(new RoleId(request.Id), cancellationToken);

        if (role is null)
        {
            return Error.NotFound();
        }

        if (!await authorization.CurrentUserHasAsync(RolePermissions.Manage, role.OwnerScopeId.Value, cancellationToken))
        {
            return Error.Forbidden();
        }

        if (request.Cascade)
        {
            foreach (User user in await users.GetWithLiveAssignmentsOfRoleAsync(request.Id, cancellationToken))
            {
                foreach (RoleAssignment assignment in user.Assignments.Where(a => a.RoleId == role.Id && !a.IsRevoked).ToList())
                {
                    user.RevokeAssignment(assignment.Id, clock.UtcNow, userContext.Id.Value);
                }
            }
        }

        roles.Remove(role);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cache.Invalidate();

        return Result.Deleted;
    }
}
