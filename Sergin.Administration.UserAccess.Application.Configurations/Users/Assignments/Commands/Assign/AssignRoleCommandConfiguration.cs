using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Assign;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments.Commands.Assign;

// Unbound and without a permission, so it adds no policy: the handler checks the permission against the scope the
// command names. It only names the texts its validation messages read their field labels from.
internal sealed class AssignRoleCommandConfiguration : ICommandConfiguration<AssignRoleCommand>
{
    public void Configure(CommandConfigurationBuilder<AssignRoleCommand> builder) =>
        builder.Texts(AssignmentTexts.Category).NotShown(command => command.UserId);
}
