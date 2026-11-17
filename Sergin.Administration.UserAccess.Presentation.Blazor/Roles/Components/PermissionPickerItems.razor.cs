using Microsoft.AspNetCore.Components;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Presentation.Blazor.Modules;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Components;

/// <summary>
/// The items of a permission <c>MudSelect</c>, grouped under module and aggregate headings, shared by the role's grant
/// picker and the create form's picker. It must sit directly inside the select, so the headings'
/// <c>HideContent</c> cascade still reaches <see cref="PermissionSubheader"/>.
/// </summary>
public sealed partial class PermissionPickerItems
{
    /// <summary>Permissions to leave out, such as those already granted; null leaves none out.</summary>
    [Parameter]
    public Func<PermissionDefinition, bool>? Exclude { get; set; }

    [Inject]
    private PermissionCatalog Catalog { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    // The modules this host shows, whose drawer group titles name the module headings.
    [Inject]
    private SerginUiModuleCatalog Modules { get; set; } = default!;

    // One heading per drawer group, so modules sharing a group show once.
    private IReadOnlyList<PermissionPickerSection> PickerSections => PermissionLabels.Sections(Catalog, Modules, Localizer);
}
