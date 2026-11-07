using Microsoft.AspNetCore.Components;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.SharedKernel.Presentation.Localizations;
using Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetAssignable;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetTree;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Assign;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetEffective;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.GetList;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Assignments.Commands.Revoke;
using Sergin.Administration.UserAccess.Presentation.Blazor.Roles;
using Sergin.Administration.UserAccess.Presentation.Blazor.Users.Assignments.Models;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Users.Assignments.Components;

/// <summary>
/// A user's assignments (FR-6, FR-12), the assign form (FR-7, FR-8, FR-10) and what they add up to (FR-11), as two
/// sections embedded in <c>UserDetailView</c>.
/// </summary>
public sealed partial class UserAssignmentsView
{
    private NewAssignmentFormModel model = new();

    private IReadOnlyCollection<UserAssignmentItem> assignments = [];
    private IReadOnlyCollection<EffectivePermissionItem> effective = [];
    private IReadOnlyCollection<ScopeTreeItem> scopes = [];
    private IReadOnlyCollection<AssignableRoleItem> roles = [];
    private bool busy;

    [Parameter]
    public Guid UserId { get; set; }

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private ICultureFormatter Formatter { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [Inject]
    private PermissionCatalog Catalog { get; set; } = default!;

    // Both date pickers say the same: a period may stay open at either end.
    private string OptionalHint => Localizer.Text(AssignmentTexts.ValidFromUtc.Hint!);

    protected override async Task OnParametersSetAsync()
    {
        ErrorOr<IReadOnlyCollection<ScopeTreeItem>> tree = await Dispatcher.SendAsync(new GetScopeTreeQueryCommand());
        scopes = tree.IsError ? [] : [.. tree.Value.OrderBy(scope => scope.Depth).ThenBy(scope => scope.Name, StringComparer.CurrentCulture)];

        await LoadAsync();
    }

    /// <summary>Reads the assignments and effective permissions again; the detail view's reload calls it.</summary>
    public async Task ReloadAsync()
    {
        await LoadAsync();

        // Called from the parent's event, which re-renders the parent only: the user id did not change.
        StateHasChanged();
    }

    private async Task LoadAsync()
    {
        ErrorOr<IReadOnlyCollection<UserAssignmentItem>> listed = await Dispatcher.SendAsync(new GetUserAssignmentsQueryCommand(UserId));
        ErrorOr<IReadOnlyCollection<EffectivePermissionItem>> held = await Dispatcher.SendAsync(new GetUserEffectivePermissionsQueryCommand(UserId));

        assignments = listed.IsError ? [] : listed.Value;
        effective = held.IsError ? [] : held.Value;
    }

    private async Task OnScopeChangedAsync(Guid scopeId)
    {
        model.ScopeId = scopeId;
        model.RoleId = Guid.Empty;

        ErrorOr<IReadOnlyCollection<AssignableRoleItem>> assignable = await Dispatcher.SendAsync(new GetAssignableRolesQueryCommand(scopeId));

        if (assignable.IsError)
        {
            roles = [];
            ErrorPresenter.Notify(assignable.FirstError);

            return;
        }

        roles = assignable.Value;
    }

    private async Task AssignAsync()
    {
        busy = true;

        ErrorOr<AssignRoleCommandResponse> result = await Dispatcher.SendAsync(new AssignRoleCommand(
            UserId,
            model.RoleId,
            model.ScopeId,
            model.IncludeSubtree,
            AsUtc(model.ValidFromUtc),
            AsUtc(model.ValidUntilUtc)));

        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);

            return;
        }

        model = new NewAssignmentFormModel { ScopeId = model.ScopeId };
        await LoadAsync();
    }

    private async Task RevokeAsync(Guid assignmentId)
    {
        busy = true;
        ErrorOr<RevokeAssignmentCommandResponse> result = await Dispatcher.SendAsync(new RevokeAssignmentCommand(UserId, assignmentId));
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.FirstError);
        }

        await LoadAsync();
    }

    // A picked calendar day is that day's UTC midnight.
    private static DateTime? AsUtc(DateTime? day) => day is { } value ? DateTime.SpecifyKind(value.Date, DateTimeKind.Utc) : null;

    private string Period(UserAssignmentItem assignment) => (assignment.ValidFromUtc, assignment.ValidUntilUtc) switch
    {
        (null, null) => Localizer.Text(AssignmentUiTexts.Always),
        ({ } from, null) => Localizer.Text(AssignmentUiTexts.From, new { From = Day(from) }),
        (null, { } until) => Localizer.Text(AssignmentUiTexts.Until, new { Until = Day(until) }),
        ({ } from, { } until) => Localizer.Text(AssignmentUiTexts.Between, new { From = Day(from), Until = Day(until) }),
    };

    private string Covers(bool includeSubtree) =>
        Localizer.Text(includeSubtree ? AssignmentUiTexts.ScopeAndBelow : AssignmentUiTexts.ScopeOnly);

    // The role's name and its owner are user data, filled into the declared shape.
    private string RoleOption(AssignableRoleItem role) =>
        Localizer.Text(AssignmentUiTexts.RoleOption, new { Role = role.Name, OwnerScope = role.OwnerScopeName });

    // Each permission by its translated label; one the catalogue no longer defines by its raw name.
    private string PermissionsOf(EffectivePermissionItem item) =>
        string.Join(", ", item.Permissions.Select(permission => PermissionLabels.Of(Catalog, Localizer, permission)));

    private string Day(DateTime utc) => Formatter.Date(new DateTimeOffset(DateTime.SpecifyKind(utc, DateTimeKind.Utc)));
}
