using MudBlazor;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Configurations.Users;
using Sergin.SharedKernel.Modules;
using Sergin.SharedKernel.Presentation.Blazor.Modules;

namespace Sergin.Administration.UserAccess.Presentation.Blazor;

/// <summary>
/// The module's drawer entries, in the shared Administration group. Each entry is also named individually so a page can
/// build its breadcrumb section step with <c>SerginBreadcrumb.Of(...)</c> from the one place the drawer reads the label
/// and href.
/// </summary>
public static class UserAccessNavigation
{
    public static SerginNavGroup Group => SerginNavGroups.Administration;

    public static SerginNavItem Users { get; } = new(
        UserUiTexts.Title,
        "/ua/users",
        Icons.Material.Filled.People,
        Order: 200,
        RequiredPermission: UserPermissions.Read);

    // Shown to a regional administrator too: the tree page lists the subtrees their scopes.read grants reach.
    public static SerginNavItem Scopes { get; } = new(
        ScopeUiTexts.Title,
        "/ua/scopes",
        Icons.Material.Filled.AccountTree,
        Order: 210,
        RequiredPermission: ScopePermissions.Read);

    public static IReadOnlyCollection<SerginNavItem> Items { get; } = [Users, Scopes];
}
