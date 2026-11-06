using Microsoft.AspNetCore.Components;
using MudBlazor;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.SharedKernel.Presentation.Errors;
using Sergin.UserAccess.Application.Configurations.Users;
using Sergin.UserAccess.Application.Contracts.Users.Commands.DeactivateUser;
using Sergin.UserAccess.Application.Contracts.Users.Commands.GetOne;
using Sergin.UserAccess.Presentation.Blazor.Users.Assignments.Components;

namespace Sergin.UserAccess.Presentation.Blazor.Users.Components;

/// <summary>One user: their name, the deactivate action, and their assignments as an embedded section.</summary>
public sealed partial class UserDetailView
{
    private UserQueryResponse? user;
    private SerginProblem? problem;
    private bool deactivating;
    private UserAssignmentsView? assignments;

    [Parameter]
    public Guid Id { get; set; }

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IDialogService DialogService { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    // A property, not a field: the tail is the page title's word until the load fills in the user name, and
    // on the not-found path it stays that way beside the problem panel.
    private IReadOnlyList<SerginBreadcrumb> Trail =>
    [
        SerginBreadcrumb.Of(UserAccessNavigation.Users),
        user is null ? new(UserTexts.Singular) : SerginBreadcrumb.Data(user.UserName),
    ];

    // Users belong to no scope, so DeactivateUser passes a caller holding Manage on any scope.
    private IReadOnlyList<SerginPageAction> Actions =>
    [
        new(UserTexts.Deactivate, Icons.Material.Outlined.PersonOff, SerginActionKind.Danger, OnClick: EventCallback.Factory.Create(this, DeactivateAsync),
            Permission: UserPermissions.Manage, AnyScope: true, Disabled: deactivating),
    ];

    protected override Task OnParametersSetAsync() => LoadAsync();

    private async Task LoadAsync()
    {
        ErrorOr<UserQueryResponse> result = await Dispatcher.SendAsync(new GetUserByIdQueryCommand(Id));

        if (result.IsError)
        {
            user = null;
            problem = ErrorPresenter.Present(result.FirstError);

            return;
        }

        problem = null;
        user = result.Value;
    }

    private async Task ReloadAsync()
    {
        await LoadAsync();

        if (assignments is not null)
        {
            await assignments.ReloadAsync();
        }
    }

    private async Task DeactivateAsync()
    {
        if (!await DialogService.ConfirmDeleteAsync(
                Localizer, UserTexts.DeactivateTitle, UserTexts.DeactivateConfirm, new { user?.UserName }, UserTexts.Deactivate))
        {
            return;
        }

        deactivating = true;

        ErrorOr<DeactivateUserCommandResponse> result = await Dispatcher.SendAsync(new DeactivateUserCommand(Id));

        deactivating = false;

        if (result.IsError)
        {
            ErrorPresenter.Notify(result.FirstError);
        }
    }
}
