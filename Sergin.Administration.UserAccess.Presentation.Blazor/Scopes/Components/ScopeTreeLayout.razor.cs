using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Delete;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetTree;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes.Components;

/// <summary>
/// The layout of the scope pages: the scopes the caller may read on the left, the page at the current route — the
/// prompt, a scope's <c>ScopeDetailView</c> or a new scope's form — on the right. A scope whose parent the caller
/// cannot reach shows as a root.
/// </summary>
public sealed partial class ScopeTreeLayout
{
    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    private static IReadOnlyList<SerginBreadcrumb> Trail { get; } = [SerginBreadcrumb.Of(UserAccessNavigation.Scopes)];

    private static string IdOf(ScopeTreeItem scope) => scope.Id.ToString();

    private static string? ParentIdOf(ScopeTreeItem scope) => scope.ParentId?.ToString();

    private static string NameOf(ScopeTreeItem scope) => scope.Name;

    private static string HrefOf(ScopeTreeItem scope) => $"/ua/scopes/{scope.Id}";

    private static string? IconOf(ScopeTreeItem scope) =>
        scope.ParentId is null ? Icons.Material.Outlined.AccountTree : Icons.Material.Outlined.FolderOpen;

    private static SerginPageAction AddChildOf(ScopeTreeItem scope) =>
        new(ScopeTexts.SubScope, Icons.Material.Outlined.Add, SerginActionKind.Secondary, Href: $"/ua/scopes/new?parentId={scope.Id}",
            Permission: ScopePermissions.Manage, AnyScope: true);

    private Task<ErrorOr<IReadOnlyCollection<ScopeTreeItem>>> LoadAsync() => Dispatcher.SendAsync(new GetScopeTreeQueryCommand());

    // A leaf only: the menu disables Delete on a scope with children, and the handler refuses one too.
    private Task<ErrorOr<Deleted>> DeleteAsync(ScopeTreeItem scope) => Dispatcher.SendAsync(new DeleteScopeCommand(scope.Id, Cascade: false));
}
