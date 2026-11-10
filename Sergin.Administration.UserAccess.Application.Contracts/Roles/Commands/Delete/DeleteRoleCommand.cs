using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Roles;

namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Delete;

/// <summary>Deletes a role. Refused while it has live assignments, unless <paramref name="Cascade"/> revokes them (BR-5).</summary>
public sealed record DeleteRoleCommand(RoleId Id, bool Cascade) : ICommand<Deleted>, IDeleteCommand<Role, RoleId>;
