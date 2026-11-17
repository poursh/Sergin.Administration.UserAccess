using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GrantPermission;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.GrantPermission;

// Requires Manage: Role is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the role's owner scope.
internal sealed class GrantRolePermissionCommandConfiguration : ICommandConfiguration<GrantRolePermissionCommand>
{
    public void Configure(CommandConfigurationBuilder<GrantRolePermissionCommand> builder)
    {
        builder.RequirePermissions(RolePermissions.Manage);
    }
}
