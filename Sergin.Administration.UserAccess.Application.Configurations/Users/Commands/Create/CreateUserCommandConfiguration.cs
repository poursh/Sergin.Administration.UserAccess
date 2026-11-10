using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.Create;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Commands.Create;

internal sealed class CreateUserCommandConfiguration : ICommandConfiguration<CreateUserCommand>
{
    public void Configure(CommandConfigurationBuilder<CreateUserCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Manage).Texts(UserTexts.Category);
}
