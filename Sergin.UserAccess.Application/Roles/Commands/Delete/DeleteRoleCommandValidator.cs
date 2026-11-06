using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.UserAccess.Application.Configurations.Roles;
using Sergin.UserAccess.Application.Contracts.Roles.Commands.Delete;

namespace Sergin.UserAccess.Application.Roles.Commands.Delete;

internal sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator(IRoleQueryRepository roles)
    {
        RuleFor(x => x.Id).NotEmpty();

        // BR-5: a role in use is deleted only with an explicit cascade, which revokes its assignments.
        RuleFor(x => x.Cascade)
            .MustAsync(async (command, _, cancellationToken) => !await roles.HasLiveAssignmentsAsync(command.Id, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(RoleTexts.InUse))
            .When(x => !x.Cascade && x.Id != Guid.Empty);
    }
}
