using Sergin.Administration.UserAccess.Application.Configurations.Users;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Users;

/// <summary>
/// The texts only the Blazor pages show. The fields, the name and the error texts are in <see cref="UserTexts"/>,
/// whose category these build on, so each key is the one the Application layer declared before.
/// </summary>
public static class UserUiTexts
{
    private static readonly TextCategory category = UserTexts.Category;

    // A text, not a field: a field named Id would also name the Id in every user command's validation messages.
    public static readonly TextDefinition Identifier = category.Text("Identifier", "Id");

    public static readonly TextDefinition Title = category.Text("Title", "Users");

    public static readonly TextDefinition New = category.Text("New", "New user");

    public static readonly TextDefinition Deactivate = category.Text("Deactivate", "Deactivate");

    public static readonly TextDefinition DeactivateTitle = category.Text("DeactivateTitle", "Deactivate user");

    public static readonly TextDefinition DeactivateConfirm =
        category.Text("DeactivateConfirm", "Deactivate {UserName}? They can no longer sign in.");
}
