using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Modules;
using Sergin.SharedKernel.Presentation.Blazor.Modules;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles;

/// <summary>One module heading of the permission pickers and the aggregate groups beneath it.</summary>
/// <param name="Title">The heading, already in the caller's culture.</param>
/// <param name="Groups">The aggregates' permissions, each under its own declared heading.</param>
internal sealed record PermissionPickerSection(string Title, IReadOnlyList<PermissionCatalogGroup> Groups);

/// <summary>What the role pages show for a permission or a module: a declared text in the caller's culture, never a raw key.</summary>
internal static class PermissionLabels
{
    private const string PlatformSchema = "sys";

    /// <summary>
    /// The module's name as its drawer group titles it; the platform's own permissions under <see cref="RoleUiTexts.PlatformModule"/>;
    /// the schema for a module this host shows no group for.
    /// </summary>
    public static string Module(SerginUiModuleCatalog modules, IStringLocalizer localizer, string schema)
    {
        if (FindModule(modules, schema) is { } found)
        {
            return localizer.Text(found.NavGroup.Title);
        }

        return string.Equals(schema, PlatformSchema, StringComparison.Ordinal) ? localizer.Text(RoleUiTexts.PlatformModule) : schema;
    }

    /// <summary>
    /// The pickers' module headings. Every schema whose module returns the same drawer group sits under one heading,
    /// so modules sharing a group (the Administration modules) show once, with all their aggregates beneath it. The
    /// headings follow the drawer: group order, then title; within a group, each module's first entry. The platform's
    /// permissions and a schema with no group in this host follow, one heading each, as <see cref="Module"/> names them.
    /// </summary>
    public static IReadOnlyList<PermissionPickerSection> Sections(
        PermissionCatalog catalog, SerginUiModuleCatalog modules, IStringLocalizer localizer)
    {
        IReadOnlyList<PermissionCatalogModule> grouped = catalog.Grouped();

        IEnumerable<PermissionPickerSection> inGroups = grouped
            .Select(module => (Module: module, Ui: FindModule(modules, module.Schema)))
            .Where(pair => pair.Ui is not null)
            .GroupBy(pair => pair.Ui!.NavGroup)
            .OrderBy(group => group.Key.Order)
            .ThenBy(group => group.Key.Title.Default, StringComparer.Ordinal)
            .Select(group => new PermissionPickerSection(
                localizer.Text(group.Key.Title),
                [
                    .. group
                        .OrderBy(pair => FirstEntryOrder(pair.Ui!))
                        .ThenBy(pair => pair.Module.Schema, StringComparer.Ordinal)
                        .SelectMany(pair => pair.Module.Groups)
                ]));

        IEnumerable<PermissionPickerSection> outsideGroups = grouped
            .Where(module => FindModule(modules, module.Schema) is null)
            .Select(module => new PermissionPickerSection(Module(modules, localizer, module.Schema), module.Groups));

        return [.. inGroups, .. outsideGroups];
    }

    /// <summary>The permission's label as the catalogue declares it (<c>PermissionCatalog.LabelOf</c>), or its raw name when the catalogue does not define it.</summary>
    public static string Of(PermissionCatalog catalog, IStringLocalizer localizer, string name) =>
        Permission.TryCreate(name, out Permission? permission) && catalog.TryGet(permission, out PermissionDefinition? definition)
            ? Of(catalog, localizer, definition)
            : name;

    /// <summary>A defined permission's label, translated.</summary>
    public static string Of(PermissionCatalog catalog, IStringLocalizer localizer, PermissionDefinition definition) =>
        localizer.Text(catalog.LabelOf(definition));

    private static ISerginWebUiModule? FindModule(SerginUiModuleCatalog modules, string schema) =>
        modules.Modules.FirstOrDefault(module => string.Equals(module.Schema, schema, StringComparison.Ordinal));

    private static int FirstEntryOrder(ISerginWebUiModule module) =>
        module.NavItems.Count == 0 ? int.MaxValue : module.NavItems.Min(item => item.Order);
}
