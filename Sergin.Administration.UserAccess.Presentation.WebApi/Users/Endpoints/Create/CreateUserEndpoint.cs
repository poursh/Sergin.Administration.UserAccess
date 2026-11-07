using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.Create;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Users.Endpoints.Create;

internal class CreateUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("/users", async ([FromBody] NewUserModel user, ISender sender) =>
            {
                ErrorOr<CreateUserCommandResponse> res = await sender.Send(
                    new CreateUserCommand(
                        new UserName(user.UserName)));

                return res.ToApiResult();
            })
            .Produces<CreateUserCommandResponse>();
    }
}
