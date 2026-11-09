using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Revoke;

public sealed record RevokeAssignmentCommand(UserInternalId RootId, RoleAssignmentId Id)
    : ICommand<RevokeAssignmentCommandResponse>, IRemoveChildCommand<User, UserInternalId, RoleAssignment, RoleAssignmentId>;
