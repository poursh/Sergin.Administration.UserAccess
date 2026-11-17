using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Create;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Delete;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetOne;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.GetTree;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Move;
using Sergin.Administration.UserAccess.Application.Contracts.Scopes.Commands.Rename;
using Sergin.Administration.UserAccess.Domain.Scopes;
using Sergin.Administration.UserAccess.Presentation.Blazor.Scopes.Components;
using MudBlazor;
using Sergin.SharedKernel.Presentation.Blazor.Aggregates;

namespace Sergin.Administration.UserAccess.Presentation.Blazor.Scopes;

/// <summary>
/// The scope's generated views, drawn inside the scope tree's pane: the detail has the name inline-editable, a parent
/// link, a move and delete only for a scope that has a parent (the root offers neither), a delete that asks whether to
/// cascade and leaves for the parent, a new child scope and a new role here as header actions, and its child scopes and
/// usable roles in its own slot. There is no generated list: the list route is the tree page.
/// </summary>
internal sealed class ScopeViewConfiguration : IAggregateViewConfiguration<Scope>
{
    public void Configure(AggregateViewBuilder<Scope> view)
    {
        view.Title(ScopeUiTexts.Title)
            .Routes("/ua/scopes", id => $"/ua/scopes/{id}", create: "/ua/scopes/new");
        view.BelongsTo<Scope, ScopeQueryResponse>(scope => scope.ParentId, scope => scope.ParentName);
        view.Tree<ScopeTreeItem>(tree => tree
            .Title(x => x.Name)
            .Icon(x => x.ParentId is null ? Icons.Material.Outlined.AccountTree : Icons.Material.Outlined.FolderOpen)
            .Filter(ScopeUiTexts.FilterPlaceholder)
            .Empty(ScopeUiTexts.TreeEmpty)
            .Prompt(ScopeUiTexts.SelectPrompt, ScopeUiTexts.SelectHint, Icons.Material.Outlined.AccountTree)
            .Children(ScopeUiTexts.ChildScopes, ScopeUiTexts.NoChildScopes));

        view.Command<CreateScopeCommand>().Label(ScopeUiTexts.NewChild);
        view.Command<RenameScopeCommand>().Label(CommonTexts.Rename).Edit(SerginEditMode.Inline);
        view.Command<MoveScopeCommand>().Label(ScopeUiTexts.Move).AppliesWhen<ScopeQueryResponse>(scope => scope.ParentId is not null);
        view.Command<DeleteScopeCommand>().ConfirmDelete(ScopeUiTexts.DeleteTitle, ScopeUiTexts.DeleteConfirm)
            .AppliesWhen<ScopeQueryResponse>(scope => scope.ParentId is not null);

        view.Detail<ScopeQueryResponse>(detail =>
            detail.Title(x => x.Name).Field(x => x.ParentName).Label(ScopeUiTexts.Under)
                .Link(scope => $"/ua/scopes/{scope.ParentId}").WhenEmpty(ScopeUiTexts.TreeRoot));
        view.Form<CreateScopeCommand>(form => form.Title(ScopeUiTexts.New));

        view.Link(ScopeUiTexts.NewRoleHere, Icons.Material.Outlined.Badge, id => $"/ua/roles/new?scopeId={id}");
        view.Slot<ScopeRolesSlot>(SerginViewSlot.DetailEntities);
    }
}
