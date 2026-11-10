using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Move;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes.Commands.Move;

// Without a permission: the handler checks the permission against the scope the command names. It only names the
// texts its validation messages read their field labels from.
// WithoutExpectedVersion: its type would require the scope's version, but a move re-paths the whole subtree, so one
// save touches several Versioned() roots and a single expected version cannot guard it.
internal sealed class MoveScopeCommandConfiguration : ICommandConfiguration<MoveScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<MoveScopeCommand> builder) =>
        builder.Texts(ScopeTexts.Category).WithoutExpectedVersion();
}
