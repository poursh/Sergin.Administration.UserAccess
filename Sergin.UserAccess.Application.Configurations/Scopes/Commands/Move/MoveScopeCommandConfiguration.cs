using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Move;

namespace Sergin.UserAccess.Application.Configurations.Scopes.Commands.Move;

// Unbound and without a permission, so it adds no policy: the handler checks the permission against the scope the
// command names. It only names the texts its validation messages read their field labels from.
internal sealed class MoveScopeCommandConfiguration : ICommandConfiguration<MoveScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<MoveScopeCommand> builder) =>
        builder.Texts(ScopeTexts.Category).NotShown(command => command.Id);
}
