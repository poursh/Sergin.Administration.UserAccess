using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.DeactivateUser;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Users.Endpoints.DeactivateUser;

internal class DeactivateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("/users/{userId:guid}/deactivate", async ([FromRoute] Guid userId, ISender sender) =>
            {
                ErrorOr<DeactivateUserCommandResponse> res = await sender.Send(
                    new DeactivateUserCommand(new UserInternalId(userId)));

                return res.ToApiResult();
            })
            .Produces<DeactivateUserCommandResponse>();
    }
}
