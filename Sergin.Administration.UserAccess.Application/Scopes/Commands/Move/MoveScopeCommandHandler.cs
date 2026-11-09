using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Application.Times;
using Sergin.Administration.UserAccess.Application.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Move;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Move;

internal sealed class MoveScopeCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IScopeRepository scopes,
    IScopeAuthorization authorization,
    IAuthorizationCacheInvalidator cache,
    IDateTimeProvider clock) : ICommandHandler<MoveScopeCommand, MoveScopeCommandResponse>
{
    public async Task<ErrorOr<MoveScopeCommandResponse>> Handle(MoveScopeCommand request, CancellationToken cancellationToken)
    {
        // Both ends: managing the subtree where it is, and where it is going.
        if (!await authorization.CurrentUserHasAsync(ScopePermissions.Manage, request.Id.Value, cancellationToken)
            || !await authorization.CurrentUserHasAsync(ScopePermissions.Manage, request.NewParentId.Value, cancellationToken))
        {
            return Error.Forbidden();
        }

        Scope? scope = await scopes.GetAsync(request.Id, cancellationToken);
        Scope? newParent = await scopes.GetAsync(request.NewParentId, cancellationToken);

        if (scope is null || newParent is null)
        {
            return Error.NotFound();
        }

        IReadOnlyCollection<Scope> subtree = await scopes.GetSubtreeAsync(scope, cancellationToken);

        ErrorOr<Success> moved = scope.MoveUnder(newParent, subtree, clock.UtcNow);

        if (moved.IsError)
        {
            return moved.Errors;
        }

        // One save: the scope and every re-pathed descendant commit together or not at all (NFR-4), with the outbox
        // row that tells other modules to re-path their scoped records.
        await unitOfWork.SaveChangesAsync(cancellationToken);
        cache.Invalidate();

        return new MoveScopeCommandResponse(scope.Id.Value);
    }
}
