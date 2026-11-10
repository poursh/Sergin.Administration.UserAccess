using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Delete;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Delete;

internal sealed class DeleteScopeCommandValidator : AbstractValidator<DeleteScopeCommand>
{
    public DeleteScopeCommandValidator(IScopeRepository scopes, IScopeQueryRepository scopeQueries)
    {
        RuleFor(x => x.Id.Value)
            .NotEmpty()
            .NotEqual(Scope.RootId.Value).WithMessage(_ => ValidationTexts.Template(ScopeTexts.RootCannotDelete))
            .OverridePropertyName(nameof(DeleteScopeCommand.Id));

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) => !await scopes.HasChildrenAsync(id, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(ScopeTexts.HasChildren))
            .When(x => x.Id.Value != Guid.Empty);

        // BR-5: in use means live assignments at it or live roles owned by it; only an explicit cascade proceeds.
        RuleFor(x => x.Cascade)
            .MustAsync(async (command, _, cancellationToken) =>
                !await scopeQueries.HasLiveAssignmentsAsync(command.Id.Value, cancellationToken)
                && !await scopeQueries.OwnsLiveRolesAsync(command.Id.Value, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(ScopeTexts.InUse))
            .When(x => !x.Cascade && x.Id.Value != Guid.Empty);
    }
}
