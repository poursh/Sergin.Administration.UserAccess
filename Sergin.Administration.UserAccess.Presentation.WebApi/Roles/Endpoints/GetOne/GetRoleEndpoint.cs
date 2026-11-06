using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetOne;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Roles.Endpoints.GetOne;

internal class GetRoleEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapGet("/roles/{roleId:guid}", async ([FromRoute] Guid roleId, ISender sender) =>
        {
            ErrorOr<RoleQueryResponse> res = await sender.Send(new GetRoleByIdQueryCommand(roleId));

            return res.ToApiResult();
        });
    }
}
