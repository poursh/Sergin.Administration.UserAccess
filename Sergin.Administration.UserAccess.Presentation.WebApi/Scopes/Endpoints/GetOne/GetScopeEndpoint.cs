using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetOne;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Scopes.Endpoints.GetOne;

internal class GetScopeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/scopes/{scopeId:guid}", async ([FromRoute] Guid scopeId, ISender sender) =>
        {
            ErrorOr<ScopeQueryResponse> res = await sender.Send(new GetScopeByIdQueryCommand(new ScopeId(scopeId)));

            return res.ToApiResult();
        });
    }
}
