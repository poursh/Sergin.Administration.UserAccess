using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GrantPermission;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.GrantPermission;

// No RequirePermissions: the permission is checked against the target scope in the handler, which is the
// record's own scope rather than the scope the caller is working in.
internal sealed class GrantRolePermissionCommandConfiguration : ICommandConfiguration<GrantRolePermissionCommand>
{
    public void Configure(CommandConfigurationBuilder<GrantRolePermissionCommand> builder)
    {
        // Its root and its expected version come from the record's type; nothing is left to declare.
    }
}
