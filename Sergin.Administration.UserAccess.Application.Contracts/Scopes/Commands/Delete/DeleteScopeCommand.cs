using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Delete;

/// <summary>
/// Deletes a leaf scope. Refused while assignments are live at it or roles are owned by it, unless
/// <paramref name="Cascade"/> is set, which revokes those assignments and deletes those roles (BR-5).
/// </summary>
public sealed record DeleteScopeCommand(ScopeId Id, bool Cascade) : ICommand<Deleted>, IDeleteCommand<Scope, ScopeId>;
