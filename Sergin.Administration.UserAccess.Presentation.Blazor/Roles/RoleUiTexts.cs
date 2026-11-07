using Sergin.Administration.UserAccess.Application.Configurations.Roles;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles;

/// <summary>
/// The texts only the Blazor pages show. The fields, the name and the error texts are in <see cref="RoleTexts"/>,
/// whose category these build on, so each key is the one the Application layer declared before.
/// </summary>
public static class RoleUiTexts
{
    private static readonly TextCategory category = RoleTexts.Category;

    public static readonly TextDefinition New = category.Text("New", "New role");

    public static readonly TextDefinition OwnedBy = category.Text("OwnedBy", "Owned by");

    public static readonly TextDefinition InheritableOnCreate =
        category.Text("InheritableOnCreate", "Inheritable: scopes below this one can use it too");

    public static readonly TextDefinition InheritableOnDetail = category.Text("InheritableOnDetail", "Scopes below the owner can use it too");

    public static readonly TextDefinition Unknown =
        category.Text("Unknown", "Unknown", "Heads the granted permissions no module defines any more.");

    public static readonly TextDefinition AddPermission = category.Text("AddPermission", "Add permission");

    public static readonly TextDefinition PlatformModule =
        category.Text("PlatformModule", "Platform", "Heads the platform's own permissions in the permission pickers.");

    public static readonly TextDefinition DeleteTitle = category.Text("DeleteTitle", "Delete role");

    public static readonly TextDefinition DeleteConfirm = category.Text("DeleteConfirm", "Delete {Name}? It can no longer be assigned.");

    public static readonly TextDefinition DeleteCascadeConfirm =
        category.Text("DeleteCascadeConfirm", "Delete {Name} and revoke every assignment of it?");
}
