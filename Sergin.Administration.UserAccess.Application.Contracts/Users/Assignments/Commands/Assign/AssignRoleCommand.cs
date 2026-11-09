using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Assign;

/// <remarks><paramref name="RoleId"/> and <paramref name="ScopeId"/> are plain references to other aggregates.</remarks>
public sealed record AssignRoleCommand(
    UserInternalId RootId, Guid RoleId, Guid ScopeId, bool IncludeSubtree, DateTime? ValidFromUtc, DateTime? ValidUntilUtc)
    : ICommand<AssignRoleCommandResponse>, IAddChildCommand<User, UserInternalId, RoleAssignment, RoleAssignmentId>;
