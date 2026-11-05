using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.UserAccess.Application.Configurations.Roles;

/// <summary>
/// The Role aggregate's texts: one field per command property a person sees (the role's name is labelled Code, as the
/// pages show it), the role pages' titles, confirms and permission pickers, and its validators' own messages.
/// </summary>
public static class RoleTexts
{
    public static readonly TextCategory Category = TextCategory.Create("UserAccess", "Roles");

    private static readonly TextCategory errors = Category.Child("Errors");

    public static readonly FieldText Name = Category.Field("Name", "Code");

    public static readonly FieldText OwnerScopeId = Category.Field("OwnerScopeId", "Owner scope");

    public static readonly FieldText IsInheritable = Category.Field("IsInheritable", "Inheritable");

    public static readonly FieldText Permissions = Category.Field("Permissions", "Permissions");

    public static readonly FieldText Permission = Category.Field("Permission", "Permission");

    public static readonly FieldText Cascade = Category.Field("Cascade", "When deleting, also revoke every assignment of this role");

    public static readonly TextDefinition Singular = Category.Text("Singular", "Role");

    public static readonly TextDefinition New = Category.Text("New", "New role");

    public static readonly TextDefinition OwnedBy = Category.Text("OwnedBy", "Owned by");

    public static readonly TextDefinition InheritableOnCreate =
        Category.Text("InheritableOnCreate", "Inheritable: scopes below this one can use it too");

    public static readonly TextDefinition InheritableOnDetail = Category.Text("InheritableOnDetail", "Scopes below the owner can use it too");

    public static readonly TextDefinition Unknown =
        Category.Text("Unknown", "Unknown", "Heads the granted permissions no module defines any more.");

    public static readonly TextDefinition AddPermission = Category.Text("AddPermission", "Add permission");

    public static readonly TextDefinition PlatformModule =
        Category.Text("PlatformModule", "Platform", "Heads the platform's own permissions in the permission pickers.");

    public static readonly TextDefinition DeleteTitle = Category.Text("DeleteTitle", "Delete role");

    public static readonly TextDefinition DeleteConfirm = Category.Text("DeleteConfirm", "Delete {Name}? It can no longer be assigned.");

    public static readonly TextDefinition DeleteCascadeConfirm =
        Category.Text("DeleteCascadeConfirm", "Delete {Name} and revoke every assignment of it?");

    /// <summary><c>CreateRoleCommandValidator</c>, <c>RenameRoleCommandValidator</c>: BR-4, the name is taken along the scope's line.</summary>
    public static readonly TextDefinition NameTakenInLine = errors.Text(
        "NameTakenInLine", "'{PropertyName}' is already used by a role in this scope, one above it or one below it.");

    /// <summary><c>CreateRoleCommandValidator</c>: a permission the catalogue does not define.</summary>
    public static readonly TextDefinition UnknownPermissions =
        errors.Text("UnknownPermissions", "'{PropertyName}' must be permissions the platform defines.");

    /// <summary><c>GrantRolePermissionCommandValidator</c>: a permission the catalogue does not define.</summary>
    public static readonly TextDefinition UnknownPermission =
        errors.Text("UnknownPermission", "'{PropertyName}' must be a permission the platform defines.");

    /// <summary>BR-1, the permission ceiling: the caller does not hold the permission in the role's owner scope.</summary>
    public static readonly TextDefinition PermissionCeilingExceeded = errors.Text(
        "PermissionCeilingExceeded",
        "You can only grant a permission you hold in the role's owner scope; you do not hold '{Permission}' there.");

    /// <summary><c>DeleteRoleCommandValidator</c>: BR-5, the role is in use and no cascade was asked for.</summary>
    public static readonly TextDefinition InUse =
        errors.Text("InUse", "The role has active assignments; delete it with cascade to revoke them too.");

    /// <summary><c>SetRoleInheritableCommandValidator</c>: BR-2, the role is assigned below its owner.</summary>
    public static readonly TextDefinition AssignedBelowOwner = errors.Text(
        "AssignedBelowOwner", "The role is assigned below its owner scope; revoke those assignments before making it non-inheritable.");
}
