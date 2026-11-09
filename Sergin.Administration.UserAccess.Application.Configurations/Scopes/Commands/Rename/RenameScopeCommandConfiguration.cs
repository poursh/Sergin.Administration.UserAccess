using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Rename;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes.Commands.Rename;

// No RequirePermissions: the permission is checked against the target scope in the handler, which is the
// record's own scope rather than the scope the caller is working in.
internal sealed class RenameScopeCommandConfiguration : ICommandConfiguration<RenameScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<RenameScopeCommand> builder)
    {
        // Its root and its expected version come from the record's type; nothing is left to declare.
    }
}
