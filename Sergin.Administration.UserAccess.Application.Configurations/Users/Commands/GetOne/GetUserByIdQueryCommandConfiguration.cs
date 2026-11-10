using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.GetOne;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Commands.GetOne;

internal sealed class GetUserByIdQueryCommandConfiguration : ICommandConfiguration<GetUserByIdQueryCommand>
{
    public void Configure(CommandConfigurationBuilder<GetUserByIdQueryCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Read).Texts(UserTexts.Category);
}
