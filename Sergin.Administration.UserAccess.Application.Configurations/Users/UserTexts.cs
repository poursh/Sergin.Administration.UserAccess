using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users;

/// <summary>
/// The User aggregate's texts: its fields and its name. Its role assignments declare theirs in <c>AssignmentTexts</c>, a
/// child category. The texts only the pages show are <c>UserUiTexts</c> and <c>AssignmentUiTexts</c>, in the Blazor RCL.
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

    public static readonly TextDefinition Singular = Category.Text("Singular", "User");

}
