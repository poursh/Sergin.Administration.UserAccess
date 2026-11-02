using Microsoft.AspNetCore.Components;

namespace Sergin.UserAccess.Presentation.Blazor.Roles.Pages;

/// <summary>The route for <see cref="Components.RoleDetailView"/>, shown as a page.</summary>
public sealed partial class RoleDetailPage
{
    [Parameter]
    public Guid Id { get; set; }
}
