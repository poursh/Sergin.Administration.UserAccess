namespace Sergin.Administration.UserAccess.Presentation.Blazor;

/// <summary>
/// The module's own texts that belong to no aggregate: its caption inside the shared Administration drawer group.
/// Only the UI shows it, so it is declared here rather than in Application.Configurations.
/// </summary>
public static class UserAccessUiTexts
{
    public static readonly TextCategory Category = TextCategory.Create("UserAccess");

    public static readonly TextDefinition MenuTitle = Category.Text("MenuTitle", "User Access");
}
