using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetList;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments.Commands.GetList;

internal sealed class GetUserAssignmentsQueryCommandConfiguration : ICommandConfiguration<GetUserAssignmentsQueryCommand>
{
    public void Configure(CommandConfigurationBuilder<GetUserAssignmentsQueryCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Read).Texts(AssignmentTexts.Category);
}
