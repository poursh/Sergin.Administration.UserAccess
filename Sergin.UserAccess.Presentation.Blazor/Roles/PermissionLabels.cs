using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain.Securities;

namespace Sergin.UserAccess.Presentation.Blazor.Roles;

/// <summary>What the role pages show for a permission or a module: localized text, never a raw key.</summary>
internal static class PermissionLabels
{
    /// <summary>The module's localized name, or its schema when no resource exists.</summary>
    public static string Module(IStringLocalizer localizer, string schema) =>
        localizer[$"Permissions.{schema}"] is { ResourceNotFound: false } text ? text.Value : schema;

    /// <summary>The permission's localized label, or its raw name when the catalogue does not define it.</summary>
    public static string Of(PermissionCatalog catalog, IStringLocalizer localizer, string name) =>
        Permission.TryCreate(name, out Permission? permission) && catalog.TryGet(permission, out PermissionDefinition? definition)
            ? localizer[definition.Label].Value
            : name;
}
