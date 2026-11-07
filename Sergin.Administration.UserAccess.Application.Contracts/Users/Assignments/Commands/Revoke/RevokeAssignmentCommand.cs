using Sergin.SharedKernel.Application.Commands;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Revoke;

public sealed record RevokeAssignmentCommand(Guid UserId, Guid AssignmentId) : ICommand<RevokeAssignmentCommandResponse>;
