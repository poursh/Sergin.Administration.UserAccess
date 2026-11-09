using FluentValidation;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Assign;

namespace Sergin.Administration.UserAccess.Application.Users.Assignments.Commands.Assign;

internal sealed class AssignRoleCommandValidator : AbstractValidator<AssignRoleCommand>
{
    public AssignRoleCommandValidator()
    {
        RuleFor(x => x.RootId.Value).NotEmpty().OverridePropertyName(nameof(AssignRoleCommand.RootId));
        RuleFor(x => x.RoleId).NotEmpty();
        RuleFor(x => x.ScopeId).NotEmpty();

        RuleFor(x => x.ValidUntilUtc)
            .GreaterThan(x => x.ValidFromUtc)
            .When(x => x.ValidFromUtc is not null && x.ValidUntilUtc is not null);
    }
}
