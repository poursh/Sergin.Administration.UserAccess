using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Move;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Scopes.Endpoints.Move;

internal class MoveScopeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/scopes/{scopeId:guid}/move", async (
            [FromRoute] Guid scopeId, [FromBody] MoveScopeModel move, ISender sender) =>
        {
            ErrorOr<MoveScopeCommandResponse> res = await sender.Send(new MoveScopeCommand(new ScopeId(scopeId), new ScopeId(move.NewParentId)));

            return res.ToApiResult();
        });
    }
}
