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
