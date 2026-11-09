using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Delete;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.Delete;

// No RequirePermissions: the permission is checked against the target scope in the handler, which is the
// record's own scope rather than the scope the caller is working in.
internal sealed class DeleteRoleCommandConfiguration : ICommandConfiguration<DeleteRoleCommand>
{
    public void Configure(CommandConfigurationBuilder<DeleteRoleCommand> builder)
    {
        // Its root and its expected version come from the record's type; nothing is left to declare.
    }
}
