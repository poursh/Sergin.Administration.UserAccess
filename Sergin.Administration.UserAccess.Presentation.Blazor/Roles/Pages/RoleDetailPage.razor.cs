using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Pages;

/// <summary>
/// A role's detail, in the scope tree's pane under its owner scope. The tree selects the owner the loaded role names,
/// so <see cref="ScopeId"/> only shapes the address.
/// </summary>
public sealed partial class RoleDetailPage
{
    /// <summary>The role's owner scope, as the address names it.</summary>
    [Parameter]
    public Guid ScopeId { get; set; }

    [Parameter]
    public Guid Id { get; set; }
}
