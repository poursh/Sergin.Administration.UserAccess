namespace Sergin.Administration.UserAccess.Domain.Users.Assignments;

/// <summary>
/// The text keys <see cref="RoleAssignment"/>'s own validation errors name in their metadata (<c>ErrorText.Of</c>). The
/// domain cannot see the declarations in <c>.Application.Configurations</c>, so each key is written here once and
/// declared there (<c>AssignmentTexts</c>); a test refuses a key with no declaration.
/// </summary>
public static class RoleAssignmentErrorKeys
{
    /// <summary>A validity period that ends before it starts.</summary>
    public const string ValidUntilNotAfterValidFrom = "user-access.users.assignments.errors.valid-until-not-after-valid-from";
}
