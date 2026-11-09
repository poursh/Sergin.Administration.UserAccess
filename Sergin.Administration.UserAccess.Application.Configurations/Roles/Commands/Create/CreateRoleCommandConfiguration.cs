using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Create;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.Create;

// Without a permission: the handler checks the permission against the scope the command names. It only names the
// texts its validation messages read their field labels from.
internal sealed class CreateRoleCommandConfiguration : ICommandConfiguration<CreateRoleCommand>
{
    public void Configure(CommandConfigurationBuilder<CreateRoleCommand> builder) =>
        builder.Texts(RoleTexts.Category);
}
