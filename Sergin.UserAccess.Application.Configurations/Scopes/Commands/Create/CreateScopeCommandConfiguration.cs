using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Create;

namespace Sergin.UserAccess.Application.Configurations.Scopes.Commands.Create;

// Unbound and without a permission, so it adds no policy: the handler checks the permission against the scope the
// command names. It only names the texts its validation messages read their field labels from.
internal sealed class CreateScopeCommandConfiguration : ICommandConfiguration<CreateScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<CreateScopeCommand> builder) =>
        builder.Texts(ScopeTexts.Category);
}
