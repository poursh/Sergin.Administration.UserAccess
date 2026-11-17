using Microsoft.AspNetCore.Components;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Components;

/// <summary>
/// The create form's permission field: a multi-select over the permission catalogue, grouped by module and aggregate.
/// It edits the command's <c>IReadOnlyCollection&lt;Permission&gt;</c> as it is; the server still refuses a permission
/// the user does not hold in the owner scope (the permission ceiling).
/// </summary>
public sealed partial class PermissionPickerInput
{
    /// <summary>The permissions picked so far, or nothing before the first pick.</summary>
    [Parameter]
    public object? Value { get; set; }

    [Parameter]
    public EventCallback<object?> ValueChanged { get; set; }

    [Parameter]
    public FormFieldView Field { get; set; } = default!;

    [Inject]
    private PermissionCatalog Catalog { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    private IReadOnlyCollection<string> Picked =>
        Value is IReadOnlyCollection<Permission> permissions ? [.. permissions.Select(permission => permission.Value)] : [];

    private string LabelOf(string? name) => name is null ? string.Empty : PermissionLabels.Of(Catalog, Localizer, name);

    private string SelectedText(IReadOnlyList<string> selected) => string.Join(", ", selected.Select(LabelOf));

    private Task PickedChangedAsync(IEnumerable<string> picked) =>
        ValueChanged.InvokeAsync((IReadOnlyCollection<Permission>)[.. picked.Select(Permission.Create)]);
}
