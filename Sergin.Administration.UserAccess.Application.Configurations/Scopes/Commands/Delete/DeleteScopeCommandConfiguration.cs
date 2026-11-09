using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Delete;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes.Commands.Delete;

// Without a permission: the handler checks the permission against the scope the command names. It only names the
// texts its validation messages read their field labels from.
// WithoutExpectedVersion: its type would require the scope's version, but a cascade also deletes the roles the scope
// owns, so one save touches several Versioned() roots and a single expected version cannot guard it.
internal sealed class DeleteScopeCommandConfiguration : ICommandConfiguration<DeleteScopeCommand>
{
    public void Configure(CommandConfigurationBuilder<DeleteScopeCommand> builder) =>
        builder.Texts(ScopeTexts.Category).WithoutExpectedVersion();
}
