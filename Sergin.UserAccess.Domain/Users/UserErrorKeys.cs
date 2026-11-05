namespace Sergin.UserAccess.Domain.Users;

/// <summary>
/// The text keys the User aggregate's own validation errors name in their metadata (<c>ErrorText.Of</c>). The domain
/// cannot see the declarations in <c>.Application.Configurations</c>, so each key is written here once and declared
/// there (<c>AssignmentTexts</c>); a test refuses a key with no declaration.
/// </summary>
public static class UserErrorKeys
{
    /// <summary>The role is not assignable at the target scope, from <see cref="User.AssignRole"/>.</summary>
    public const string RoleNotAssignableHere = "user-access.users.assignments.errors.role-not-assignable-here";

    /// <summary>The user already holds the role at the scope, from <see cref="User.AssignRole"/>.</summary>
    public const string RoleAlreadyHeld = "user-access.users.assignments.errors.role-already-held";
}
