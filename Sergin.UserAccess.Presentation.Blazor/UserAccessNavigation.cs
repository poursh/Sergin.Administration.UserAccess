using MudBlazor;
using Sergin.SharedKernel.Modules;
using Sergin.UserAccess.Application.Configurations;
using Sergin.UserAccess.Application.Configurations.Scopes;
using Sergin.UserAccess.Application.Configurations.Users;

namespace Sergin.UserAccess.Presentation.Blazor;

/// <summary>
/// The module's drawer group and entries. Each entry is also named individually so a page can build its breadcrumb
/// section step with <c>SerginBreadcrumb.Of(...)</c> from the one place the drawer reads the label and href.
/// </summary>
public static class UserAccessNavigation
{
    public static SerginNavGroup Group { get; } = new(
        UserAccessTexts.MenuTitle,
        Icons.Material.Filled.AdminPanelSettings,
        Order: 200);

    public static SerginNavItem Users { get; } = new(
        UserTexts.Title,
        "/ua/users",
        Icons.Material.Filled.People,
        Order: 200,
        RequiredPermission: UserPermissions.Read);

    // Shown to a regional administrator too: the tree page lists the subtrees their scopes.read grants reach.
    public static SerginNavItem Scopes { get; } = new(
        ScopeTexts.Title,
        "/ua/scopes",
        Icons.Material.Filled.AccountTree,
        Order: 210,
        RequiredPermission: ScopePermissions.Read);

    public static IReadOnlyCollection<SerginNavItem> Items { get; } = [Users, Scopes];
}
