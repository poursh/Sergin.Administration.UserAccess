using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.UserAccess.Application.Contracts.Roles.Commands.Create;

namespace Sergin.UserAccess.Application.Configurations.Roles.Commands.Create;

// Unbound and without a permission, so it adds no policy: the handler checks the permission against the scope the
// command names. It only names the texts its validation messages read their field labels from.
internal sealed class CreateRoleCommandConfiguration : ICommandConfiguration<CreateRoleCommand>
{
    public void Configure(CommandConfigurationBuilder<CreateRoleCommand> builder) =>
        builder.Texts(RoleTexts.Category);
}
