using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Rename;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes.Commands.Rename;

// No RequirePermissions: the permission is checked against the target scope in the handler, which is the
// record's own scope rather than the scope the caller is working in.
internal sealed class RenameScopeCommandConfiguration : ICommandConfiguration<RenameScopeCommand, Scope>
{
    public void Configure(CommandConfigurationBuilder<RenameScopeCommand> builder) =>
        builder.RequireExpectedVersion().NotShown(command => command.Id);
}
