using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.SetInheritable;

// Requires Manage: Role is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the role's owner scope.
internal sealed class SetRoleInheritableCommandConfiguration : ICommandConfiguration<SetRoleInheritableCommand>
{
    public void Configure(CommandConfigurationBuilder<SetRoleInheritableCommand> builder)
    {
        builder.RequirePermissions(RolePermissions.Manage);
    }
}
