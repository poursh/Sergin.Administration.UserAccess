using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.UserAccess.Application.Configurations.Scopes;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Move;

namespace Sergin.UserAccess.Application.Scopes.Commands.Move;

internal sealed class MoveScopeCommandValidator : AbstractValidator<MoveScopeCommand>
{
    public MoveScopeCommandValidator(IScopeQueryRepository scopes)
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NewParentId).NotEmpty();

        // BR-6, AC-8: a move may not leave an assignment outside its role's owner subtree.
        RuleFor(x => x.NewParentId)
            .MustAsync(async (command, newParentId, cancellationToken) =>
                !await scopes.MoveWouldStrandAssignmentsAsync(command.Id, newParentId, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(ScopeTexts.MoveStrandsAssignments))
            .When(x => x.Id != Guid.Empty && x.NewParentId != Guid.Empty);
    }
}
