using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Users.Pages;

/// <summary>The route for <see cref="Components.UserDetailView"/>, shown as a page.</summary>
public sealed partial class UserDetailPage
{
    [Parameter]
    public Guid Id { get; set; }
}
