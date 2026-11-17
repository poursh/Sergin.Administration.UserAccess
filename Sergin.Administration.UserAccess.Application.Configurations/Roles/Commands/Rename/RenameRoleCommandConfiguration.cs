using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Rename;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.Rename;

// Requires Manage: Role is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the role's owner scope.
internal sealed class RenameRoleCommandConfiguration : ICommandConfiguration<RenameRoleCommand>
{
    public void Configure(CommandConfigurationBuilder<RenameRoleCommand> builder)
    {
        builder.RequirePermissions(RolePermissions.Manage);
    }
}
