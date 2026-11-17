using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Create;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Delete;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Move;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes;

/// <summary>
/// The scope's routes and the few texts the registry demands, so a role can name its owner scope (<c>BelongsTo</c>).
/// The scope pages themselves are not generated yet; that conversion completes this configuration.
/// </summary>
internal sealed class ScopeViewConfiguration : IAggregateViewConfiguration<Scope>
{
    public void Configure(AggregateViewBuilder<Scope> view)
    {
        view.Title(ScopeUiTexts.Title)
            .Routes("/ua/scopes", id => $"/ua/scopes/{id}", create: "/ua/scopes/new");

        view.Command<CreateScopeCommand>().Label(ScopeUiTexts.New);
        view.Command<MoveScopeCommand>().Label(ScopeUiTexts.Move);
        view.Command<DeleteScopeCommand>().ConfirmDelete(ScopeUiTexts.DeleteTitle, ScopeUiTexts.DeleteConfirm);

        view.Detail<ScopeQueryResponse>(detail => detail.Title(x => x.Name).Field(x => x.ParentName).Label(ScopeUiTexts.Under));
    }
}
