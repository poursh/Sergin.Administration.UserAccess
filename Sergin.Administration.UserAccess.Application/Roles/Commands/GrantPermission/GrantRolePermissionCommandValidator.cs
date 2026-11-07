using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GrantPermission;

namespace Sergin.Administration.UserAccess.Application.Roles.Commands.GrantPermission;

internal sealed class GrantRolePermissionCommandValidator : AbstractValidator<GrantRolePermissionCommand>
{
    public GrantRolePermissionCommandValidator(PermissionCatalog catalog)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Permission).NotNull();

        // Advisory like the other repository rules: the catalogue is what the platform defines today. Revoke
        // has no such rule, so a name a release dropped can still be taken off a role.
        RuleFor(x => x.Permission)
            .Must(permission => catalog.TryGet(permission, out _))
            .WithMessage(_ => ValidationTexts.Template(RoleTexts.UnknownPermission))
            .When(x => x.Permission is not null);
    }
}
