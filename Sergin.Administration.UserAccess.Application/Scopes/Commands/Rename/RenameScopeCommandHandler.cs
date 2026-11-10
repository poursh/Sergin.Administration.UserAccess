using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Rename;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Rename;

internal sealed class RenameScopeCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IScopeRepository scopes,
    IScopeAuthorization authorization) : ICommandHandler<RenameScopeCommand, RenameScopeCommandResponse>
{
    public async Task<ErrorOr<RenameScopeCommandResponse>> Handle(RenameScopeCommand request, CancellationToken cancellationToken)
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

        scope.Rename(request.Name);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new RenameScopeCommandResponse(scope.Id.Value);
    }
}
