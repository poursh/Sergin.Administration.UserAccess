using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Rename;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes.Commands.Rename;

// Requires Manage: Scope is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the target scope.
internal sealed class RenameScopeCommandConfiguration : ICommandConfiguration<RenameScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<RenameScopeCommand> builder) =>
        builder.RequirePermissions(ScopePermissions.Manage);
}
