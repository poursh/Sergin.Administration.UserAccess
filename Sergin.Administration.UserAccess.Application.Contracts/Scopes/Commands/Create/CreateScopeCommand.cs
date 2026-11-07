using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Create;

public sealed record CreateScopeCommand(Guid ParentId, ScopeName Name) : ICommand<CreateScopeCommandResponse>;
