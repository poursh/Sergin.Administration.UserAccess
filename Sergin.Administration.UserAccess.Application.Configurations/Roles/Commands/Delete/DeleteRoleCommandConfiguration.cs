using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Delete;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.Delete;

// No RequirePermissions: the permission is checked against the target scope in the handler, which is the
// record's own scope rather than the scope the caller is working in.
internal sealed class DeleteRoleCommandConfiguration : ICommandConfiguration<DeleteRoleCommand, Role>
{
    public void Configure(CommandConfigurationBuilder<DeleteRoleCommand> builder) =>
        builder.RequireExpectedVersion().NotShown(command => command.Id);
}
