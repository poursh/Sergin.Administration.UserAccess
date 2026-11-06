using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.UserAccess.Application.Configurations.Scopes;

/// <summary>
/// The Scope aggregate's texts: one field per command property a person sees, the scope tree and the scope pages, and
/// the errors raised about a scope, by its validators and by <c>Scope.MoveUnder</c> (named through
/// <c>ScopeErrorKeys</c>).
/// </summary>
public static class ScopeTexts
{
    public static readonly TextCategory Category = TextCategory.Create("UserAccess", "Scopes");

    private static readonly TextCategory errors = Category.Child("Errors");

    public static readonly FieldText Name = Category.Field("Name", "Name");

    public static readonly FieldText ParentId = Category.Field("ParentId", "Parent scope");

    public static readonly FieldText NewParentId = Category.Field("NewParentId", "Move under");

    public static readonly FieldText Cascade =
        Category.Field("Cascade", "When deleting, also revoke its assignments and delete the roles it owns");

    public static readonly TextDefinition Title = Category.Text("Title", "Scopes");

    public static readonly TextDefinition Singular = Category.Text("Singular", "Scope");

    public static readonly TextDefinition New = Category.Text("New", "New scope");

    public static readonly TextDefinition Under = Category.Text("Under", "Under");

    public static readonly TextDefinition TreeRoot = Category.Text("TreeRoot", "The root of the tree");

    public static readonly TextDefinition NewChild = Category.Text("NewChild", "New child scope");

    public static readonly TextDefinition NewRoleHere = Category.Text("NewRoleHere", "New role here");

    public static readonly TextDefinition SubScope = Category.Text("SubScope", "Sub-scope");

    public static readonly TextDefinition Move = Category.Text("Move", "Move");

    public static readonly TextDefinition ChildScopes = Category.Text("ChildScopes", "Child scopes");

    public static readonly TextDefinition NoChildScopes = Category.Text("NoChildScopes", "No child scopes.");

    public static readonly TextDefinition NoScopes = Category.Text("NoScopes", "No scopes.");

    public static readonly TextDefinition Level = Category.Text("Level", "Level");

    public static readonly TextDefinition RolesUsableHere = Category.Text("RolesUsableHere", "Roles usable here");

    public static readonly TextDefinition Role = Category.Text("Role", "Role");

    public static readonly TextDefinition OwnedBy = Category.Text("OwnedBy", "Owned by");

    public static readonly TextDefinition Inheritable = Category.Text("Inheritable", "Inheritable");

    public static readonly TextDefinition Yes = Category.Text("Yes", "Yes");

    public static readonly TextDefinition No = Category.Text("No", "No");

    public static readonly TextDefinition FilterPlaceholder = Category.Text("FilterPlaceholder", "Filter scopes…");

    public static readonly TextDefinition TreeEmpty = Category.Text("TreeEmpty", "No scopes you can see.");

    public static readonly TextDefinition SelectPrompt = Category.Text("SelectPrompt", "Select a scope");

    public static readonly TextDefinition SelectHint =
        Category.Text("SelectHint", "Its details open here. Add a sub-scope with the tree's actions.");

    public static readonly TextDefinition DeleteTitle = Category.Text("DeleteTitle", "Delete scope");

    public static readonly TextDefinition DeleteConfirm =
        Category.Text("DeleteConfirm", "Delete {Name}? It disappears from the tree and every scope picker.");

    /// <summary><c>Scope.MoveUnder</c>: the root has no parent to leave (<c>ScopeErrorKeys.RootCannotMove</c>).</summary>
    public static readonly TextDefinition RootCannotMove = errors.Text("RootCannotMove", "The root scope cannot be moved.");

    /// <summary><c>Scope.MoveUnder</c>: the target is the scope or below it (<c>ScopeErrorKeys.MoveUnderItself</c>).</summary>
    public static readonly TextDefinition MoveUnderItself =
        errors.Text("MoveUnderItself", "A scope cannot be moved under itself or one of its descendants.");

    /// <summary><c>MoveScopeCommandValidator</c>: BR-6, the move would strand an assignment.</summary>
    public static readonly TextDefinition MoveStrandsAssignments = errors.Text(
        "MoveStrandsAssignments", "The move would leave an assignment outside the subtree of the scope that owns its role.");

    /// <summary><c>DeleteScopeCommandValidator</c>: the root is never deleted.</summary>
    public static readonly TextDefinition RootCannotDelete = errors.Text("RootCannotDelete", "The root scope cannot be deleted.");

    /// <summary><c>DeleteScopeCommandValidator</c>: only a leaf is deleted.</summary>
    public static readonly TextDefinition HasChildren =
        errors.Text("HasChildren", "A scope with child scopes cannot be deleted; delete or move its children first.");

    /// <summary><c>DeleteScopeCommandValidator</c>: BR-5, the scope is in use and no cascade was asked for.</summary>
    public static readonly TextDefinition InUse = errors.Text(
        "InUse", "The scope has active assignments or owns roles; delete it with cascade to revoke and delete them too.");
}
