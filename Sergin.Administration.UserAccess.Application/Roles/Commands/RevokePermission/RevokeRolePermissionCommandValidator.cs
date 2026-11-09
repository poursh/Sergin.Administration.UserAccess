using FluentValidation;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.RevokePermission;

namespace Sergin.Administration.UserAccess.Application.Roles.Commands.RevokePermission;

internal sealed class RevokeRolePermissionCommandValidator : AbstractValidator<RevokeRolePermissionCommand>
{
    public RevokeRolePermissionCommandValidator()
    {
        RuleFor(x => x.Id.Value).NotEmpty().OverridePropertyName(nameof(RevokeRolePermissionCommand.Id));
        RuleFor(x => x.Permission).NotNull();
    }
}
