using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.RevokePermission;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.RevokePermission;

// Requires Manage: Role is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the role's owner scope.
internal sealed class RevokeRolePermissionCommandConfiguration : ICommandConfiguration<RevokeRolePermissionCommand>
{
    public void Configure(CommandConfigurationBuilder<RevokeRolePermissionCommand> builder)
    {
        builder.RequirePermissions(RolePermissions.Manage);
    }
}
