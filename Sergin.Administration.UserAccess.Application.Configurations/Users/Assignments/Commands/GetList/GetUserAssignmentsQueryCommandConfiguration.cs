using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetList;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments.Commands.GetList;

internal sealed class GetUserAssignmentsQueryCommandConfiguration : ICommandConfiguration<GetUserAssignmentsQueryCommand, User>
{
    public void Configure(CommandConfigurationBuilder<GetUserAssignmentsQueryCommand> builder) =>
        builder.RequirePermissions(UserPermissions.Read).Texts(AssignmentTexts.Category).NotShown(command => command.UserId);
}
