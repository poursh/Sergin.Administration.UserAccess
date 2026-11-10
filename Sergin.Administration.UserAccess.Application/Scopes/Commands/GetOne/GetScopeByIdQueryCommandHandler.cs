using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.GetOne;

internal sealed class GetScopeByIdQueryCommandHandler(
    IScopeQueryRepository repository, IScopeAuthorization authorization, ConcurrencyContext concurrency)
    : VersionedQueryHandler<GetScopeByIdQueryCommand, ScopeQueryResponse>(concurrency)
{
    public override async Task<ErrorOr<Versioned<ScopeQueryResponse>>> HandleVersioned(
        GetScopeByIdQueryCommand request, CancellationToken cancellationToken)
    {
        if (!await authorization.CurrentUserHasAsync(ScopePermissions.Read, request.Id.Value, cancellationToken))
        {
            return Error.Forbidden();
        }

        Versioned<ScopeQueryResponse>? scope = await repository.GetScopeById(request.Id.Value, cancellationToken);

        return scope is null ? Error.NotFound() : scope;
    }
}
