using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sergin.SharedKernel.Presentation.WebApi.Endpoints.Results;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;

namespace Sergin.Administration.UserAccess.Presentation.WebApi.Roles.Endpoints.SetInheritable;

internal class SetRoleInheritableEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPut("/roles/{roleId:guid}/inheritable", async (
            [FromRoute] Guid roleId, [FromBody] SetRoleInheritableModel role, ISender sender) =>
        {
            ErrorOr<SetRoleInheritableCommandResponse> res = await sender.Send(new SetRoleInheritableCommand(roleId, role.IsInheritable));

            return res.ToApiResult();
        });
    }
}
