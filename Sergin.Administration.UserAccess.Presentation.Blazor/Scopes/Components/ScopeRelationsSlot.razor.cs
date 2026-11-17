using Microsoft.AspNetCore.Components;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetAssignable;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetTree;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes.Components;

/// <summary>
/// A scope's child scopes and the roles usable at it, drawn by the generated scope detail view in its entities slot.
/// The slot reads the tree and the assignable roles itself, again whenever the view's generation moves (a reload, a
/// rename, a move), so they follow what the view shows.
/// </summary>
public sealed partial class ScopeRelationsSlot
{
    private IReadOnlyCollection<AssignableRoleItem> roles = [];
    private IReadOnlyCollection<ScopeTreeItem> tree = [];
    private Guid loadedFor;
    private int loadedGeneration = -1;

    /// <summary>The scope detail view's context: the loaded scope, its id and the generation.</summary>
    [Parameter]
    public AggregateViewContext Context { get; set; } = default!;

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    protected override async Task OnParametersSetAsync()
    {
        if (Context.Record is not ScopeQueryResponse || Context.TargetIds[0] == loadedFor && Context.Generation == loadedGeneration)
        {
            return;
        }

        loadedFor = Context.TargetIds[0];
        loadedGeneration = Context.Generation;

        ErrorOr<IReadOnlyCollection<AssignableRoleItem>> assignable = await Dispatcher.SendAsync(new GetAssignableRolesQueryCommand(loadedFor));
        roles = assignable.IsError ? [] : assignable.Value;

        ErrorOr<IReadOnlyCollection<ScopeTreeItem>> loadedTree = await Dispatcher.SendAsync(new GetScopeTreeQueryCommand());
        tree = loadedTree.IsError ? [] : loadedTree.Value;
    }
}
