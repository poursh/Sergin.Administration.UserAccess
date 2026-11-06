using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.Administration.UserAccess.Application.Configurations.Users;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.GetList;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Users.Components;

public sealed partial class UserListView
{
    private SerginAggregateList<GetUserListItem> list = default!;

    private IReadOnlyList<SerginBreadcrumb> Trail { get; } = [SerginBreadcrumb.Of(UserAccessNavigation.Users)];

    // Users belong to no scope, so CreateUser passes a caller holding Manage on any scope.
    private IReadOnlyList<SerginPageAction> Actions { get; } =
    [
        new(UserTexts.New, Icons.Material.Filled.Add, SerginActionKind.Primary, Href: "/ua/users/new",
            Permission: UserPermissions.Manage, AnyScope: true),
    ];

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    private Task<ErrorOr<ListQueryResponse<GetUserListItem>>> LoadAsync(SerginListRequest request, CancellationToken cancellationToken) =>
        Dispatcher.SendAsync(new GetUserListQueryCommand(request.Paggination), cancellationToken);

    private Task ReloadAsync() => list.ReloadAsync();
}
