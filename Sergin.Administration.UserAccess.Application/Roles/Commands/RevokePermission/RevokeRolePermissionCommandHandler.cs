using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.RevokePermission;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;

namespace Sergin.Administration.UserAccess.Application.Roles.Commands.RevokePermission;

internal sealed class RevokeRolePermissionCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IRoleRepository roles,
    IScopeAuthorization authorization,
    IAuthorizationCacheInvalidator cache) : ICommandHandler<RevokeRolePermissionCommand, RevokeRolePermissionCommandResponse>
{
    public async Task<ErrorOr<RevokeRolePermissionCommandResponse>> Handle(RevokeRolePermissionCommand request, CancellationToken cancellationToken)
    {
        Role? role = await roles.GetAsync(request.Id, cancellationToken);

        if (role is null)
        {
            return Error.NotFound();
        }

        if (!await authorization.CurrentUserHasAsync(RolePermissions.Manage, role.OwnerScopeId.Value, cancellationToken))
        {
            return Error.Forbidden();
        }

        role.Revoke(request.Permission);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cache.Invalidate();

        return new RevokeRolePermissionCommandResponse(role.Id.Value);
    }
}
