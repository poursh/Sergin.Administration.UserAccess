using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments;

/// <summary>
/// The texts of a user's role assignments, a category nested under <see cref="UserTexts"/>: the assign form's fields,
/// the assignments and effective-permissions sections, and the errors <c>User.Assign</c> and <c>RoleAssignment</c>
/// raise (named through <c>UserErrorKeys</c> and <c>RoleAssignmentErrorKeys</c>).
/// </summary>
public static class AssignmentTexts
{
    public static readonly TextCategory Category = UserTexts.Category.Child("Assignments");

    private static readonly TextCategory errors = Category.Child("Errors");

    public static readonly FieldText RoleId = Category.Field("RoleId", "Role");

    public static readonly FieldText ScopeId = Category.Field("ScopeId", "Scope");

    public static readonly FieldText IncludeSubtree = Category.Field("IncludeSubtree", "Also every scope below");

    public static readonly FieldText ValidFromUtc = Category.Field("ValidFromUtc", "Valid from", hint: "Optional");

    public static readonly FieldText ValidUntilUtc = Category.Field("ValidUntilUtc", "Valid until", hint: "Optional");

    public static readonly TextDefinition Title = Category.Text("Title", "Assignments");

    public static readonly TextDefinition Empty = Category.Text("Empty", "No assignments yet.");

    public static readonly TextDefinition Covers = Category.Text("Covers", "Covers");

    public static readonly TextDefinition Valid = Category.Text("Valid", "Valid");

    public static readonly TextDefinition ScopeAndBelow = Category.Text("ScopeAndBelow", "Scope and below");

    public static readonly TextDefinition ScopeOnly = Category.Text("ScopeOnly", "This scope only");

    public static readonly TextDefinition Always = Category.Text("Always", "Always");

    public static readonly TextDefinition From = Category.Text("From", "From {From}", "An assignment valid from a day on.");

    public static readonly TextDefinition Until = Category.Text("Until", "Until {Until}", "An assignment valid up to a day.");

    public static readonly TextDefinition Between =
        Category.Text("Between", "{From} to {Until}", "An assignment valid between two days.");

    public static readonly TextDefinition Revoke = Category.Text("Revoke", "Revoke");

    public static readonly TextDefinition Revoked = Category.Text("Revoked", "Revoked");

    public static readonly TextDefinition AssignTitle = Category.Text("AssignTitle", "Assign a role");

    public static readonly TextDefinition Assign = Category.Text("Assign", "Assign");

    public static readonly TextDefinition RoleOption = Category.Text(
        "RoleOption", "{Role} ({OwnerScope})", "A role in the assign form's picker, then the scope that owns it.");

    public static readonly TextDefinition EffectiveTitle = Category.Text("EffectiveTitle", "Effective permissions");

    public static readonly TextDefinition NoPermissions = Category.Text("NoPermissions", "No permissions held.");

    public static readonly TextDefinition Permissions = Category.Text("Permissions", "Permissions");

    /// <summary><c>User.Assign</c>: the role is not usable at the target scope (<c>UserErrorKeys.RoleNotAssignableHere</c>).</summary>
    public static readonly TextDefinition RoleNotAssignableHere = errors.Text(
        "RoleNotAssignableHere", "The role can only be assigned at its owner scope, or below it when the role is inheritable.");

    /// <summary><c>User.Assign</c>: a live assignment of the role at the scope exists (<c>UserErrorKeys.RoleAlreadyHeld</c>).</summary>
    public static readonly TextDefinition RoleAlreadyHeld = errors.Text("RoleAlreadyHeld", "The user already holds this role at this scope.");

    /// <summary>
    /// <c>RoleAssignment</c>: the period ends before it starts (<c>RoleAssignmentErrorKeys.ValidUntilNotAfterValidFrom</c>).
    /// </summary>
    public static readonly TextDefinition ValidUntilNotAfterValidFrom =
        errors.Text("ValidUntilNotAfterValidFrom", "'Valid until' must be after 'Valid from'.");
}
