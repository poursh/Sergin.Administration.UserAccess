using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.DeactivateUser;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Commands.DeactivateUser;

internal sealed class DeactivateUserCommandConfiguration : ICommandConfiguration<DeactivateUserCommand, User>
{
    public void Configure(CommandConfigurationBuilder<DeactivateUserCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Manage).Texts(UserTexts.Category).NotShown(command => command.Id);
}
