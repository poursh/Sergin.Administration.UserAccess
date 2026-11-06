using Sergin.SharedKernel.Application.Commands;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Rename;

public sealed record RenameScopeCommand(Guid Id, ScopeName Name) : ICommand<RenameScopeCommandResponse>;
