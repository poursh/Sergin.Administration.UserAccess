using Microsoft.AspNetCore.Components;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetOne;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes.Components;

/// <summary>The scope detail's parent: the parent's name linking to its detail, or the root text for the root scope.</summary>
public sealed partial class ScopeParentValue
{
    /// <summary>What the generated view hands a field component: the loaded scope, in <c>Record</c>.</summary>
    [Parameter]
    public AggregateViewContext? Context { get; set; }

    /// <summary>The field's value: the parent's name, null for the root.</summary>
    [Parameter]
    public object? Value { get; set; }

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    private (Guid Id, string Name)? Parent =>
        Context?.Record is ScopeQueryResponse { ParentId: { } parentId } scope ? (parentId, scope.ParentName ?? Value as string ?? string.Empty) : null;
}
