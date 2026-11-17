using Microsoft.AspNetCore.Components;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Pages;

/// <summary>The route for the generated role create form, shown as a page.</summary>
public sealed partial class CreateRolePage
{
    /// <summary>The scope that owns the new role; the root when the link names none.</summary>
    [SupplyParameterFromQuery]
    public Guid ScopeId { get; set; }

    protected override void OnParametersSet()
    {
        if (ScopeId == Guid.Empty)
        {
            ScopeId = Scope.RootId.Value;
        }
    }
}
