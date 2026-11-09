using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;

namespace Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetList;

/// <summary>Every assignment the user has ever held, live first, revoked ones kept for history.</summary>
public sealed record GetUserAssignmentsQueryCommand(UserInternalId RootId)
    : IQuery<IReadOnlyCollection<UserAssignmentItem>>, IChildListOf<User, UserInternalId, RoleAssignment>;
