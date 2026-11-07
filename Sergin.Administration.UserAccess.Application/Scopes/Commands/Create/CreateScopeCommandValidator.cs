using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Common;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Create;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Create;

internal sealed class CreateScopeCommandValidator : AbstractValidator<CreateScopeCommand>
{
    public CreateScopeCommandValidator(IScopeQueryRepository scopes)
    {
        RuleFor(x => x.ParentId).NotEmpty();

        RuleFor(x => x.Name.Value)
            .NotEmpty()
            .MaximumLength(ScopeName.MaxLength)
            .OverridePropertyName(nameof(CreateScopeCommand.Name));

        // Advisory: ix_scopes_parent_id_name is the guarantee under a race.
        RuleFor(x => x.Name)
            .MustAsync((command, name, cancellationToken) =>
                NotTakenAsync(scopes, command.ParentId, name.Value, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(CommonValidationTexts.MustBeUniqueIn))
            .When(x => x.ParentId != Guid.Empty && !string.IsNullOrWhiteSpace(x.Name.Value));
    }

    private static async Task<bool> NotTakenAsync(IScopeQueryRepository scopes, Guid parentId, string name, CancellationToken cancellationToken) =>
        !await scopes.IsSiblingNameTakenAsync(parentId, name, excludeScopeId: null, cancellationToken);
}
