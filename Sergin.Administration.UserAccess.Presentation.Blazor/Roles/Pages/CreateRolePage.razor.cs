using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Pages;

/// <summary>The route for generated role create form, shown as a page.</summary>
public sealed partial class CreateRolePage
{
    /// <summary>The scope that owns the new role.</summary>
    [SupplyParameterFromQuery]
    public Guid? ScopeId { get; set; }
}
