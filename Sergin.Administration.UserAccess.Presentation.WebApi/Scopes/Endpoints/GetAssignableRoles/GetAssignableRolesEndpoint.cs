using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetAssignable;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Scopes.Endpoints.GetAssignableRoles;

internal class GetAssignableRolesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("/scopes/{scopeId:guid}/assignable-roles", async ([FromRoute] Guid scopeId, ISender sender) =>
            {
                ErrorOr<IReadOnlyCollection<AssignableRoleItem>> res = await sender.Send(new GetAssignableRolesQueryCommand(scopeId));

                return res.ToApiResult();
            })
            .Produces<IReadOnlyCollection<AssignableRoleItem>>();
    }
}
