using FluentValidation;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Revoke;

namespace Sergin.Administration.UserAccess.Application.Users.Assignments.Commands.Revoke;

internal sealed class RevokeAssignmentCommandValidator : AbstractValidator<RevokeAssignmentCommand>
{
    public RevokeAssignmentCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.AssignmentId).NotEmpty();
    }
}
