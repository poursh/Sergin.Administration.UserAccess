using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users;

/// <summary>
/// The User aggregate's texts: its fields and the user pages' titles and confirms. Its role assignments declare theirs
/// in <c>AssignmentTexts</c>, a child category.
/// </summary>
/// <remarks>
/// The User aggregate names no category with <c>Texts(...)</c>; each user command's configuration names this one
/// instead. A root's category also keys its permission labels, and <c>permission.ua.users.manage</c> and
/// <c>permission.ua.assignments.manage</c> would then share <c>user-access.users.permissions.manage</c>.
/// </remarks>
public static class UserTexts
{
    public static readonly TextCategory Category = TextCategory.Create("UserAccess", "Users");

    public static readonly FieldText UserName = Category.Field("UserName", "User name");

    // A text, not a field: a field named Id would also name the Id in every user command's validation messages.
    public static readonly TextDefinition Identifier = Category.Text("Identifier", "Id");

    public static readonly TextDefinition Title = Category.Text("Title", "Users");

    public static readonly TextDefinition Singular = Category.Text("Singular", "User");

    public static readonly TextDefinition New = Category.Text("New", "New user");

    public static readonly TextDefinition Deactivate = Category.Text("Deactivate", "Deactivate");

    public static readonly TextDefinition DeactivateTitle = Category.Text("DeactivateTitle", "Deactivate user");

    public static readonly TextDefinition DeactivateConfirm =
        Category.Text("DeactivateConfirm", "Deactivate {UserName}? They can no longer sign in.");
}
