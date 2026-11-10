using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Move;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Move;

internal sealed class MoveScopeCommandValidator : AbstractValidator<MoveScopeCommand>
{
    public MoveScopeCommandValidator(IScopeQueryRepository scopes)
    {
        RuleFor(x => x.Id.Value).NotEmpty().OverridePropertyName(nameof(MoveScopeCommand.Id));
        RuleFor(x => x.NewParentId.Value).NotEmpty().OverridePropertyName(nameof(MoveScopeCommand.NewParentId));

        // BR-6, AC-8: a move may not leave an assignment outside its role's owner subtree.
        RuleFor(x => x.NewParentId)
            .MustAsync(async (command, newParentId, cancellationToken) =>
                !await scopes.MoveWouldStrandAssignmentsAsync(command.Id.Value, newParentId.Value, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(ScopeTexts.MoveStrandsAssignments))
            .When(x => x.Id.Value != Guid.Empty && x.NewParentId.Value != Guid.Empty);
    }
}
