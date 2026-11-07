using Microsoft.AspNetCore.Components;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes.Pages;

/// <summary>The scope tree's own route: nothing selected yet, so the pane prompts for a scope.</summary>
public sealed partial class ScopeTreePage
{
    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;
}
