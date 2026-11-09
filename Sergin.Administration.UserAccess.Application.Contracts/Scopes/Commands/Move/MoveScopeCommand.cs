using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Move;

public sealed record MoveScopeCommand(ScopeId Id, ScopeId NewParentId) : ICommand<MoveScopeCommandResponse>, IMoveCommand<Scope, ScopeId>;
