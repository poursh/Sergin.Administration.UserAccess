using Microsoft.AspNetCore.Components;
using Sergin.UserAccess.Domain.Scopes;

namespace Sergin.UserAccess.Presentation.Blazor.Scopes.Pages;

/// <summary>A new scope, in the scope tree's pane, under the parent the link names.</summary>
public sealed partial class CreateScopePage
{
    /// <summary>The scope the new one goes under; the root when the link names none.</summary>
    [SupplyParameterFromQuery]
    public Guid ParentId { get; set; }

    protected override void OnParametersSet()
    {
        if (ParentId == Guid.Empty)
        {
            ParentId = Scope.RootId.Value;
        }
    }
}
