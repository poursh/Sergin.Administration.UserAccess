namespace Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetOne;

public sealed record RoleQueryResponse(
    Guid Id, Guid OwnerScopeId, string OwnerScopeName, string Name, bool IsInheritable, IReadOnlyCollection<string> Permissions);
