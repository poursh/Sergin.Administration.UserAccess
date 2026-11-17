using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Pages;

/// <summary>The route for generated role detail view, shown as a page.</summary>
public sealed partial class RoleDetailPage
{
    [Parameter]
    public Guid Id { get; set; }
}
