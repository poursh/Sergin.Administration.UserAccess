using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.UserAccess.Application.Configurations.Roles;
using Sergin.UserAccess.Application.Contracts.Roles.Commands.SetInheritable;

namespace Sergin.UserAccess.Application.Roles.Commands.SetInheritable;

internal sealed class SetRoleInheritableCommandValidator : AbstractValidator<SetRoleInheritableCommand>
{
    public SetRoleInheritableCommandValidator(IRoleQueryRepository roles)
    {
        RuleFor(x => x.Id).NotEmpty();

        // BR-2 holds for existing assignments too: a role assigned below its owner must stay inheritable.
        RuleFor(x => x.IsInheritable)
            .MustAsync(async (command, _, cancellationToken) =>
                !await roles.HasLiveAssignmentsAwayFromOwnerAsync(command.Id, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(RoleTexts.AssignedBelowOwner))
            .When(x => !x.IsInheritable && x.Id != Guid.Empty);
    }
}
