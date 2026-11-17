using Sergin.SharedKernel.Application.Commands.Configuration;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Create;

namespace Sergin.Administration.UserAccess.Application.Configurations.Roles.Commands.Create;

// Requires Manage: Role is not Scoped(), so the pipeline checks it on any scope, and the handler still checks it on
// the role's owner scope.
internal sealed class CreateRoleCommandConfiguration : ICommandConfiguration<CreateRoleCommand>
{
    public void Configure(CommandConfigurationBuilder<CreateRoleCommand> builder) =>
        builder.RequirePermissions(RolePermissions.Manage).Texts(RoleTexts.Category);
}
