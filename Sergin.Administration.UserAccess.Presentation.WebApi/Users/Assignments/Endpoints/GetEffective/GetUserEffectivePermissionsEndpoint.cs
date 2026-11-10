using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetEffective;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Users.Assignments.Endpoints.GetEffective;

internal class GetUserEffectivePermissionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("/users/{userId:guid}/effective-permissions", async ([FromRoute] Guid userId, ISender sender) =>
            {
                ErrorOr<IReadOnlyCollection<EffectivePermissionItem>> res = await sender.Send(new GetUserEffectivePermissionsQueryCommand(new UserInternalId(userId)));

                return res.ToApiResult();
            })
            .Produces<IReadOnlyCollection<EffectivePermissionItem>>();
    }
}
