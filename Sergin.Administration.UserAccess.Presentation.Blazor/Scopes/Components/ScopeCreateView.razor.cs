using Microsoft.AspNetCore.Components;
using Sergin.SharedKernel.Presentation.Blazor.Errors;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Create;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Presentation.Blazor.Scopes.Models;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes.Components;

/// <summary>The form for a new scope under <see cref="ParentId"/>; saving opens the new scope.</summary>
public sealed partial class ScopeCreateView
{
    private readonly ScopeNameFormModel model = new();

    private bool submitting;
    private Func<object, string, Task<IEnumerable<string>>> validation = default!;

    /// <summary>The scope the new one goes under.</summary>
    [Parameter]
    [EditorRequired]
    public Guid ParentId { get; set; }

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

    private IReadOnlyList<SerginBreadcrumb> Trail { get; } =
    [
        SerginBreadcrumb.Of(UserAccessNavigation.Scopes),
        new(ScopeTexts.New),
    ];

    protected override void OnInitialized() => validation = FormValidator.RulesFor(ToCommand);

    // One mapping for both the field-by-field validation and the submit, so the two cannot drift.
    private CreateScopeCommand ToCommand() => new(ParentId, new ScopeName(model.Name));

    private async Task SubmitAsync()
    {
        submitting = true;

        ErrorOr<CreateScopeCommandResponse> result = await Dispatcher.SendAsync(ToCommand());

        submitting = false;

        if (result.IsError)
        {
            // Every error, not the first: validation yields one per broken rule.
            ErrorPresenter.Notify(result.Errors);

            return;
        }

        await Host.LeaveAsync($"/ua/scopes/{result.Value.Id}");
    }
}
