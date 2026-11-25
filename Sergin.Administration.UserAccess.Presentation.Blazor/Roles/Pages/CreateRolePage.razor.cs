using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Pages;

/// <summary>A new role, in the scope tree's pane, owned by the scope the route names.</summary>
public sealed partial class CreateRolePage
{
    /// <summary>The scope that owns the new role.</summary>
    [Parameter]
    public Guid ScopeId { get; set; }
}
