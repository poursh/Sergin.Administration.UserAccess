using MudBlazor;
using Sergin.Administration.UserAccess.Application.Configurations.Users.Assignments;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Domain.Users.Assignments;
using Sergin.Administration.UserAccess.Presentation.Blazor.Users.Assignments.Components;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Users;

/// <summary>
/// The user's generated views: its routes and titles, the identifier shown on the list and the detail (an id is hidden
/// by default), a Deactivate that asks first, and the assignments drawn by their own section component.
/// </summary>
internal sealed class UserViewConfiguration : IAggregateViewConfiguration<User>
{
    // After UserName, whose place is its declaration index (1).
    private const int IdentifierOrder = 2;

    public void Configure(AggregateViewBuilder<User> view)
    {
        view.Title(UserUiTexts.Title)
            .Routes("/ua/users", id => $"/ua/users/{id}", create: "/ua/users/new");

        view.Command<CreateUserCommand>().Label(UserUiTexts.New);
        view.Command<DeactivateUserCommand>()
            .Label(UserUiTexts.Deactivate)
            .Icon(Icons.Material.Outlined.PersonOff)
            .Kind(SerginActionKind.Danger)
            .Confirm(UserUiTexts.DeactivateTitle, UserUiTexts.DeactivateConfirm, UserUiTexts.Deactivate);

        // The name first (the list's link), then the id: the order the responses declare them in is the other way round.
        view.Detail<UserQueryResponse>(detail =>
            detail.Title(x => x.UserName).Field(x => x.Id).Shown().Label(UserUiTexts.Identifier).Order(IdentifierOrder));
        view.List<GetUserListItem>(list =>
            list.Title(x => x.UserName).Field(x => x.Id).Shown().Label(UserUiTexts.Identifier).Order(IdentifierOrder));
        view.Form<CreateUserCommand>(form => form.Title(UserUiTexts.New));

        view.Child<RoleAssignment>(child => child.Named(AssignmentTexts.Singular).Section<UserAssignmentsSection>());
    }
}
