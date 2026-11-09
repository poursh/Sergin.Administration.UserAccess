using FluentValidation;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Rename;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Scopes.Commands.Rename;

internal sealed class RenameScopeCommandValidator : AbstractValidator<RenameScopeCommand>
{
    public RenameScopeCommandValidator()
    {
        RuleFor(x => x.Id.Value).NotEmpty().OverridePropertyName(nameof(RenameScopeCommand.Id));

        RuleFor(x => x.Name.Value)
            .NotEmpty()
            .MaximumLength(ScopeName.MaxLength)
            .OverridePropertyName(nameof(RenameScopeCommand.Name));
    }
}
