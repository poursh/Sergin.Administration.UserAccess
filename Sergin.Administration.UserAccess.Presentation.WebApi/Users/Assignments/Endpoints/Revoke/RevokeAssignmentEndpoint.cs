using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Revoke;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Users.Assignments.Endpoints.Revoke;

internal class RevokeAssignmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapDelete("/users/{userId:guid}/assignments/{assignmentId:guid}", async (
            [FromRoute] Guid userId, [FromRoute] Guid assignmentId, ISender sender) =>
        {
            ErrorOr<RevokeAssignmentCommandResponse> res = await sender.Send(new RevokeAssignmentCommand(new UserInternalId(userId), new RoleAssignmentId(assignmentId)));

            return res.ToApiResult();
        });
    }
}
