using Microsoft.AspNetCore.Components;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.GrantPermission;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.RevokePermission;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.SharedKernel.Application.Concurrency;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Domain;
using Sergin.SharedKernel.Domain.Securities;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;
using Sergin.SharedKernel.Presentation.Blazor.Errors;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Roles.Components;

/// <summary>
/// A role's granted permissions with the grant picker, drawn by the generated role detail view in its entities slot.
/// Every grant and revoke is sent at the version the view holds and the view reads the role again afterwards, so a
/// stale answer leaves it showing the current role.
/// </summary>
public sealed partial class RolePermissionsSlot
{
    private string? permissionToGrant;
    private bool busy;

    /// <summary>The role detail view's context: the loaded role, the version it was loaded at and a way to reload it.</summary>
    [Parameter]
    public AggregateViewContext Context { get; set; } = default!;

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private PermissionCatalog Catalog { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    private IReadOnlyCollection<string> Granted => (Context.Record as RoleQueryResponse)?.Permissions ?? [];

    // The role's grants the catalogue defines, shown by label; a grant it no longer defines is listed apart by its
    // raw name, still revocable.
    private IEnumerable<PermissionDefinition> GrantedDefined => Granted.Select(TryDefine).OfType<PermissionDefinition>();

    private IReadOnlyCollection<string> GrantedUnknown => [.. Granted.Where(permission => TryDefine(permission) is null)];

    private RoleId Role => new(Context.TargetIds[0]);

    private string LabelOf(string? name) => name is null ? string.Empty : PermissionLabels.Of(Catalog, Localizer, name);

    // A stored row that is not even a well-formed name is unknown too, not a render failure.
    private PermissionDefinition? TryDefine(string permission) =>
        Permission.TryCreate(permission, out Permission? parsed) && Catalog.TryGet(parsed, out PermissionDefinition? definition)
            ? definition
            : null;

    private bool IsGranted(PermissionDefinition definition) => Granted.Contains(definition.Permission.Value);

    private Task GrantPickedAsync() => permissionToGrant is null ? Task.CompletedTask : GrantAsync(permissionToGrant);

    // Public for the tests, which send a grant and a revoke as the picker and a chip's close do.
    public Task GrantAsync(string permission) => SendGuardedAsync(new GrantRolePermissionCommand(Role, Permission.Create(permission)));

    public Task RevokeAsync(string permission) => SendGuardedAsync(new RevokeRolePermissionCommand(Role, Permission.Create(permission)));

    private async Task SendGuardedAsync<TResponse>(IRequest<ErrorOr<TResponse>> command)
    {
        if (Context.Version is not { } expected)
        {
            return;
        }

        busy = true;
        ErrorOr<Versioned<TResponse>> result = await Dispatcher.SendVersionedAsync(command, expected);
        busy = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.Errors);
        }

        permissionToGrant = null;
        await Context.ReloadAsync();
    }
}
