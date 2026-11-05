using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.UserAccess.Application.Configurations.Scopes;
using Sergin.UserAccess.Application.Contracts.Scopes.Commands.Delete;
using Sergin.UserAccess.Domain.Scopes;

namespace Sergin.UserAccess.Application.Scopes.Commands.Delete;

internal sealed class DeleteScopeCommandValidator : AbstractValidator<DeleteScopeCommand>
{
    public DeleteScopeCommandValidator(IScopeRepository scopes, IScopeQueryRepository scopeQueries)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotEqual(Scope.RootId.Value).WithMessage(_ => ValidationTexts.Template(ScopeTexts.RootCannotDelete));

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) => !await scopes.HasChildrenAsync(new ScopeId(id), cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(ScopeTexts.HasChildren))
            .When(x => x.Id != Guid.Empty);

        // BR-5: in use means live assignments at it or live roles owned by it; only an explicit cascade proceeds.
        RuleFor(x => x.Cascade)
            .MustAsync(async (command, _, cancellationToken) =>
                !await scopeQueries.HasLiveAssignmentsAsync(command.Id, cancellationToken)
                && !await scopeQueries.OwnsLiveRolesAsync(command.Id, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(ScopeTexts.InUse))
            .When(x => !x.Cascade && x.Id != Guid.Empty);
    }
}
