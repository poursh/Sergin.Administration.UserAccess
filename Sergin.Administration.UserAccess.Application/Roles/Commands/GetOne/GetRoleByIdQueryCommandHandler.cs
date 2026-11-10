using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;

namespace Sergin.Administration.UserAccess.Application.Roles.Commands.GetOne;

internal sealed class GetRoleByIdQueryCommandHandler(
    IRoleQueryRepository repository, IScopeAuthorization authorization, ConcurrencyContext concurrency)
    : VersionedQueryHandler<GetRoleByIdQueryCommand, RoleQueryResponse>(concurrency)
{
    public override async Task<ErrorOr<Versioned<RoleQueryResponse>>> HandleVersioned(
        GetRoleByIdQueryCommand request, CancellationToken cancellationToken)
    {
        Versioned<RoleQueryResponse>? role = await repository.GetRoleById(request.Id.Value, cancellationToken);

        if (role is null)
        {
            return Error.NotFound();
        }

        if (!await authorization.CurrentUserHasAsync(RolePermissions.Read, role.Value.OwnerScopeId, cancellationToken))
        {
            return Error.Forbidden();
        }

        return role;
    }
}
