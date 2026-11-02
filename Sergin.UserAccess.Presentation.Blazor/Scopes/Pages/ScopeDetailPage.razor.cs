using Microsoft.AspNetCore.Components;

namespace Sergin.UserAccess.Presentation.Blazor.Scopes.Pages;

public sealed partial class ScopeDetailPage
{
    [Parameter]
    public Guid Id { get; set; }
}
