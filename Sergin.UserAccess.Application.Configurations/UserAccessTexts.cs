using Sergin.SharedKernel.Application.Localizations;

namespace Sergin.UserAccess.Application.Configurations;

/// <summary>The module's own texts that belong to no aggregate: its menu title (the drawer group).</summary>
public static class UserAccessTexts
{
    public static readonly TextCategory Category = TextCategory.Create("UserAccess");

    public static readonly TextDefinition MenuTitle = Category.Text("MenuTitle", "User Access");
}
