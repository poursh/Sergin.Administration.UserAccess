using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Presentation.Blazor.Modules;
using Sergin.UserAccess.Application.Configurations.Roles;

namespace Sergin.UserAccess.Presentation.Blazor.Roles;

/// <summary>What the role pages show for a permission or a module: a declared text in the caller's culture, never a raw key.</summary>
internal static class PermissionLabels
{
    private const string PlatformSchema = "sys";

    /// <summary>
    /// The module's name as its drawer group titles it; the platform's own permissions under <see cref="RoleTexts.PlatformModule"/>;
    /// the schema for a module this host shows no group for.
    /// </summary>
    public static string Module(SerginUiModuleCatalog modules, IStringLocalizer localizer, string schema)
    {
        if (modules.Modules.FirstOrDefault(module => string.Equals(module.Schema, schema, StringComparison.Ordinal)) is { } found)
        {
            return localizer.Text(found.NavGroup.Title);
        }

        return string.Equals(schema, PlatformSchema, StringComparison.Ordinal) ? localizer.Text(RoleTexts.PlatformModule) : schema;
    }

    /// <summary>The permission's label as the catalogue declares it (<c>PermissionCatalog.LabelOf</c>), or its raw name when the catalogue does not define it.</summary>
    public static string Of(PermissionCatalog catalog, IStringLocalizer localizer, string name) =>
        Permission.TryCreate(name, out Permission? permission) && catalog.TryGet(permission, out PermissionDefinition? definition)
            ? Of(catalog, localizer, definition)
            : name;

    /// <summary>A defined permission's label, translated.</summary>
    public static string Of(PermissionCatalog catalog, IStringLocalizer localizer, PermissionDefinition definition) =>
        localizer.Text(catalog.LabelOf(definition));
}
