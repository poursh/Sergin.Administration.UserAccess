namespace Sergin.UserAccess.Domain.Scopes;

/// <summary>
/// The text keys the Scope aggregate's own validation errors name in their metadata (<c>ErrorText.Of</c>). The domain
/// cannot see the declarations in <c>.Application.Configurations</c>, so each key is written here once and declared
/// there (<c>ScopeTexts</c>); a test refuses a key with no declaration.
/// </summary>
public static class ScopeErrorKeys
{
    /// <summary>The root scope cannot be moved, from <see cref="Scope.MoveUnder"/>.</summary>
    public const string RootCannotMove = "user-access.scopes.errors.root-cannot-move";

    /// <summary>A scope cannot go under itself or a descendant, from <see cref="Scope.MoveUnder"/>.</summary>
    public const string MoveUnderItself = "user-access.scopes.errors.move-under-itself";
}
