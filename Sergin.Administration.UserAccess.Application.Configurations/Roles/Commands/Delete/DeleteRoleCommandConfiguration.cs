using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Delete;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.Delete;

// Requires Manage: Role is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the role's owner scope.
internal sealed class DeleteRoleCommandConfiguration : ICommandConfiguration<DeleteRoleCommand>
{
    public void Configure(CommandConfigurationBuilder<DeleteRoleCommand> builder)
    {
        builder.RequirePermissions(RolePermissions.Manage);
    }
}
