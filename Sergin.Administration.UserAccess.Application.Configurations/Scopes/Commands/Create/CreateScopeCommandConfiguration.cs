using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Create;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes.Commands.Create;

// Requires Manage: Scope is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the target scope.
// It also names the texts its validation messages read their field labels from.
internal sealed class CreateScopeCommandConfiguration : ICommandConfiguration<CreateScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<CreateScopeCommand> builder) =>
        builder.Texts(ScopeTexts.Category).RequirePermissions(ScopePermissions.Manage);
}
