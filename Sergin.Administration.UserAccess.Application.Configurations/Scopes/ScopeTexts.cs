using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes;

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

    public static readonly TextDefinition Singular = Category.Text("Singular", "Scope");

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
