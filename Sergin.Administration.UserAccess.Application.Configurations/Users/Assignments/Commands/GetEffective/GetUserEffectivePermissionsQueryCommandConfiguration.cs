using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetEffective;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments.Commands.GetEffective;

internal sealed class GetUserEffectivePermissionsQueryCommandConfiguration : ICommandConfiguration<GetUserEffectivePermissionsQueryCommand>
{
    public void Configure(CommandConfigurationBuilder<GetUserEffectivePermissionsQueryCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Read).Texts(AssignmentTexts.Category);
}
