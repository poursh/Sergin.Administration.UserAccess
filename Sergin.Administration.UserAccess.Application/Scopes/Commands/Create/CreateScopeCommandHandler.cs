using Sergin.SharedKernel.Application.Commands;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Authorization;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Create;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Create;

internal sealed class CreateScopeCommandHandler(
    IUserAccessUnitOfWork unitOfWork,
    IScopeRepository scopes,
    IScopeAuthorization authorization,
    IAuthorizationCacheInvalidator cache) : ICommandHandler<CreateScopeCommand, CreateScopeCommandResponse>
{
    public async Task<ErrorOr<CreateScopeCommandResponse>> Handle(CreateScopeCommand request, CancellationToken cancellationToken)
    {
        if (!await authorization.CurrentUserHasAsync(ScopePermissions.Manage, request.ParentId.Value, cancellationToken))
        {
            return Error.Forbidden();
        }

        Scope? parent = await scopes.GetAsync(request.ParentId, cancellationToken);

        if (parent is null)
        {
            return Error.NotFound();
        }

        var scope = Scope.Create(request.Name, parent);
        scopes.Insert(scope);

        await unitOfWork.SaveChangesAsync(cancellationToken);
        cache.Invalidate();

        return new CreateScopeCommandResponse(scope.Id.Value);
    }
}
