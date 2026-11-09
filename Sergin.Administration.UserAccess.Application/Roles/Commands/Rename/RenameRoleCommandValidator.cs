using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Rename;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Roles.Commands.Rename;

internal sealed class RenameRoleCommandValidator : AbstractValidator<RenameRoleCommand>
{
    public RenameRoleCommandValidator(IRoleQueryRepository roles)
    {
        RuleFor(x => x.Id.Value).NotEmpty().OverridePropertyName(nameof(RenameRoleCommand.Id));

        RuleFor(x => x.Name.Value)
            .NotEmpty()
            .MaximumLength(RoleName.MaxLength)
            .OverridePropertyName(nameof(RenameRoleCommand.Name));

        RuleFor(x => x.Name)
            .MustAsync(async (command, name, cancellationToken) =>
                !await roles.IsNameTakenInLineOfRoleAsync(command.Id.Value, name.Value, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(RoleTexts.NameTakenInLine))
            .When(x => x.Id.Value != Guid.Empty && !string.IsNullOrWhiteSpace(x.Name.Value));
    }
}
