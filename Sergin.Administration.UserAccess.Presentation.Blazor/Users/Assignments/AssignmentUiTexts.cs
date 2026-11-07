using Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Users.Assignments;

/// <summary>
/// The texts only the Blazor pages show. The fields, the name and the error texts are in <see cref="AssignmentTexts"/>,
/// whose category these build on, so each key is the one the Application layer declared before.
/// </summary>
public static class AssignmentUiTexts
{
    private static readonly TextCategory category = AssignmentTexts.Category;

    public static readonly TextDefinition Title = category.Text("Title", "Assignments");

    public static readonly TextDefinition Empty = category.Text("Empty", "No assignments yet.");

    public static readonly TextDefinition Covers = category.Text("Covers", "Covers");

    public static readonly TextDefinition Valid = category.Text("Valid", "Valid");

    public static readonly TextDefinition ScopeAndBelow = category.Text("ScopeAndBelow", "Scope and below");

    public static readonly TextDefinition ScopeOnly = category.Text("ScopeOnly", "This scope only");

    public static readonly TextDefinition Always = category.Text("Always", "Always");

    public static readonly TextDefinition From = category.Text("From", "From {From}", "An assignment valid from a day on.");

    public static readonly TextDefinition Until = category.Text("Until", "Until {Until}", "An assignment valid up to a day.");

    public static readonly TextDefinition Between =
        category.Text("Between", "{From} to {Until}", "An assignment valid between two days.");

    public static readonly TextDefinition Revoke = category.Text("Revoke", "Revoke");

    public static readonly TextDefinition Revoked = category.Text("Revoked", "Revoked");

    public static readonly TextDefinition AssignTitle = category.Text("AssignTitle", "Assign a role");

    public static readonly TextDefinition Assign = category.Text("Assign", "Assign");

    public static readonly TextDefinition RoleOption = category.Text(
        "RoleOption", "{Role} ({OwnerScope})", "A role in the assign form's picker, then the scope that owns it.");

    public static readonly TextDefinition EffectiveTitle = category.Text("EffectiveTitle", "Effective permissions");

    public static readonly TextDefinition NoPermissions = category.Text("NoPermissions", "No permissions held.");

    public static readonly TextDefinition Permissions = category.Text("Permissions", "Permissions");
}
