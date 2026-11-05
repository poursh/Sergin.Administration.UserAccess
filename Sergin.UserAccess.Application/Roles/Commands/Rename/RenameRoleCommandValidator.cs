using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.UserAccess.Application.Configurations.Roles;
using Sergin.UserAccess.Application.Contracts.Roles.Commands.Rename;
using Sergin.UserAccess.Domain.Roles;

namespace Sergin.UserAccess.Application.Roles.Commands.Rename;

internal sealed class RenameRoleCommandValidator : AbstractValidator<RenameRoleCommand>
{
    public RenameRoleCommandValidator(IRoleQueryRepository roles)
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name.Value)
            .NotEmpty()
            .MaximumLength(RoleName.MaxLength)
            .OverridePropertyName(nameof(RenameRoleCommand.Name));

        RuleFor(x => x.Name)
            .MustAsync(async (command, name, cancellationToken) =>
                !await roles.IsNameTakenInLineOfRoleAsync(command.Id, name.Value, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(RoleTexts.NameTakenInLine))
            .When(x => x.Id != Guid.Empty && !string.IsNullOrWhiteSpace(x.Name.Value));
    }
}
