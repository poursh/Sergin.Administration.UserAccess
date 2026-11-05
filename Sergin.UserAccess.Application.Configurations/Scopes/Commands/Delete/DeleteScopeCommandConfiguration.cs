using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Delete;

namespace Sergin.UserAccess.Application.Configurations.Scopes.Commands.Delete;

// Unbound and without a permission, so it adds no policy: the handler checks the permission against the scope the
// command names. It only names the texts its validation messages read their field labels from.
internal sealed class DeleteScopeCommandConfiguration : ICommandConfiguration<DeleteScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<DeleteScopeCommand> builder) =>
        builder.Texts(ScopeTexts.Category).NotShown(command => command.Id);
}
