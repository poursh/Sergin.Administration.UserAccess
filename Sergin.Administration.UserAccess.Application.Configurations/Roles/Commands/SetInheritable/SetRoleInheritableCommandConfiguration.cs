using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.SetInheritable;

// No RequirePermissions: the permission is checked against the target scope in the handler, which is the
// record's own scope rather than the scope the caller is working in.
internal sealed class SetRoleInheritableCommandConfiguration : ICommandConfiguration<SetRoleInheritableCommand, Role>
{
    public void Configure(CommandConfigurationBuilder<SetRoleInheritableCommand> builder) =>
        builder.RequireExpectedVersion().NotShown(command => command.Id);
}
