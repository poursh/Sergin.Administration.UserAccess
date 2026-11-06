using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.GetOne;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Commands.GetOne;

internal sealed class GetUserByIdQueryCommandConfiguration : ICommandConfiguration<GetUserByIdQueryCommand, User>
{
    public void Configure(CommandConfigurationBuilder<GetUserByIdQueryCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Read).Texts(UserTexts.Category).NotShown(command => command.Id);
}
