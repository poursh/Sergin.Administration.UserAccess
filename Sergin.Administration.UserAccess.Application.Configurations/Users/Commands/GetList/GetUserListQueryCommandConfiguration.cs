using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.GetList;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Commands.GetList;

internal sealed class GetUserListQueryCommandConfiguration : ICommandConfiguration<GetUserListQueryCommand>
{
    public void Configure(CommandConfigurationBuilder<GetUserListQueryCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Read).Texts(UserTexts.Category);
}
