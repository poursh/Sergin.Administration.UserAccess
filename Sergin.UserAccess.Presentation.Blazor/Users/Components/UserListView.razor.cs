using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.UserAccess.Application.Configurations.Users;
using Sergin.UserAccess.Application.Contracts.Users.Commands.GetList;

namespace Sergin.UserAccess.Presentation.Blazor.Users.Components;

public sealed partial class UserListView
{
    private SerginAggregateList<GetUserListItem> list = default!;

    private IReadOnlyList<SerginBreadcrumb> Trail { get; } = [SerginBreadcrumb.Of(UserAccessNavigation.Users)];

    // Users belong to no scope, so CreateUser passes a caller holding Manage on any scope.
    private IReadOnlyList<SerginPageAction> Actions { get; } =
    [
        new("New user", Icons.Material.Filled.Add, SerginActionKind.Primary, Href: "/ua/users/new",
            Permission: UserPermissions.Manage, AnyScope: true),
    ];

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    private Task<ErrorOr<ListQueryResponse<GetUserListItem>>> LoadAsync(SerginListRequest request, CancellationToken cancellationToken) =>
        Dispatcher.SendAsync(new GetUserListQueryCommand(request.Paggination), cancellationToken);

    private Task ReloadAsync() => list.ReloadAsync();
}
