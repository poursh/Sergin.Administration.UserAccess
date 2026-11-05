using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.UserAccess.Application.Contracts.Users.Assignments.Commands.Revoke;

namespace Sergin.UserAccess.Application.Configurations.Users.Assignments.Commands.Revoke;

// Unbound and without a permission, so it adds no policy: the handler checks the permission against the scope the
// command names. It only names the texts its validation messages read their field labels from.
internal sealed class RevokeAssignmentCommandConfiguration : ICommandConfiguration<RevokeAssignmentCommand>
{
    public void Configure(CommandConfigurationBuilder<RevokeAssignmentCommand> builder) =>
        builder.Texts(AssignmentTexts.Category).NotShown(command => command.UserId).NotShown(command => command.AssignmentId);
}
