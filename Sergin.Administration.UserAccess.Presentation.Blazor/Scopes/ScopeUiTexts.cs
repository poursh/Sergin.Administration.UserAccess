using Sergin.Administration.UserAccess.Application.Configurations.Scopes;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes;

/// <summary>
/// The texts only the Blazor pages show. The fields, the name and the error texts are in <see cref="ScopeTexts"/>,
/// whose category these build on, so each key is the one the Application layer declared before.
/// </summary>
public static class ScopeUiTexts
{
    private static readonly TextCategory category = ScopeTexts.Category;

    public static readonly TextDefinition Title = category.Text("Title", "Scopes");

    public static readonly TextDefinition New = category.Text("New", "New scope");

    public static readonly TextDefinition Under = category.Text("Under", "Under");

    public static readonly TextDefinition TreeRoot = category.Text("TreeRoot", "The root of the tree");

    public static readonly TextDefinition NewChild = category.Text("NewChild", "New child scope");

    public static readonly TextDefinition NewRoleHere = category.Text("NewRoleHere", "New role here");

    public static readonly TextDefinition SubScope = category.Text("SubScope", "Sub-scope");

    public static readonly TextDefinition Move = category.Text("Move", "Move");

    public static readonly TextDefinition ChildScopes = category.Text("ChildScopes", "Child scopes");

    public static readonly TextDefinition NoChildScopes = category.Text("NoChildScopes", "No child scopes.");

    public static readonly TextDefinition NoScopes = category.Text("NoScopes", "No scopes.");

    public static readonly TextDefinition Level = category.Text("Level", "Level");

    public static readonly TextDefinition RolesUsableHere = category.Text("RolesUsableHere", "Roles usable here");

    public static readonly TextDefinition Role = category.Text("Role", "Role");

    public static readonly TextDefinition OwnedBy = category.Text("OwnedBy", "Owned by");

    public static readonly TextDefinition Inheritable = category.Text("Inheritable", "Inheritable");

    public static readonly TextDefinition Yes = category.Text("Yes", "Yes");

    public static readonly TextDefinition No = category.Text("No", "No");

    public static readonly TextDefinition FilterPlaceholder = category.Text("FilterPlaceholder", "Filter scopes…");

    public static readonly TextDefinition TreeEmpty = category.Text("TreeEmpty", "No scopes you can see.");

    public static readonly TextDefinition SelectPrompt = category.Text("SelectPrompt", "Select a scope");

    public static readonly TextDefinition SelectHint =
        category.Text("SelectHint", "Its details open here. Add a sub-scope with the tree's actions.");

    public static readonly TextDefinition DeleteTitle = category.Text("DeleteTitle", "Delete scope");

    public static readonly TextDefinition DeleteConfirm =
        category.Text("DeleteConfirm", "Delete {Name}? It disappears from the tree and every scope picker.");
}
