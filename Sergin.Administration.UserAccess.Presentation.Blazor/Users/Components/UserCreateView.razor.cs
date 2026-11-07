using Microsoft.AspNetCore.Components;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.Administration.UserAccess.Application.Configurations.Users;
using Sergin.Administration.UserAccess.Application.Contracts.Users.Commands.Create;
using Sergin.Administration.UserAccess.Domain.Users;
using Sergin.Administration.UserAccess.Presentation.Blazor.Users.Models;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Users.Components;

/// <summary>The form for a new user; saving opens the new user.</summary>
public sealed partial class UserCreateView
{
    private readonly NewUserFormModel model = new();

    private bool submitting;
    private Func<object, string, Task<IEnumerable<string>>> validation = default!;

    private IReadOnlyList<SerginBreadcrumb> Trail { get; } =
    [
        SerginBreadcrumb.Of(UserAccessNavigation.Users),
        new(UserTexts.New),
    ];

    [Inject]
    private ISerginDispatcher Dispatcher { get; set; } = default!;

    [Inject]
    private ISerginFormValidator FormValidator { get; set; } = default!;

    [Inject]
    private IUiErrorPresenter ErrorPresenter { get; set; } = default!;

    [Inject]
    private IStringLocalizer Localizer { get; set; } = default!;

    [CascadingParameter]
    private SerginViewHost Host { get; set; } = SerginViewHost.Unhosted;

    // Created on this view, so it re-renders after SubmitAsync; SerginFormView invokes it once every field is valid.
    private EventCallback OnSubmit => EventCallback.Factory.Create(this, SubmitAsync);

    protected override void OnInitialized() => validation = FormValidator.RulesFor(ToCommand);

    // One mapping for both the field-by-field validation and the submit, so the two cannot drift.
    private CreateUserCommand ToCommand() => new(new UserName(model.UserName));

    private async Task SubmitAsync()
    {
        submitting = true;

        ErrorOr<CreateUserCommandResponse> result = await Dispatcher.SendAsync(ToCommand());

        submitting = false;

        if (result.IsError)
        {
            // Every error, not the first: validation yields one per broken rule.
            ErrorPresenter.Notify(result.Errors);

            return;
        }

        await Host.LeaveAsync($"/ua/users/{result.Value.Id}");
    }
}
