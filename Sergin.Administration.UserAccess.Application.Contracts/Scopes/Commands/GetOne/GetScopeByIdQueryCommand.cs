using Sergin.SharedKernel.Application.Commands.Queries;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetOne;

public sealed record GetScopeByIdQueryCommand(ScopeId Id) : IQuery<ScopeQueryResponse>, IGetOneQuery<Scope, ScopeId>;
