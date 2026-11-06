using FluentValidation;
using Sergin.SharedKernel.Application.Localizations.Validation;
using Sergin.SharedKernel.Application.Securities.Authorization;
using Sergin.SharedKernel.Application.Validations;
using Sergin.Administration.UserAccess.Application.Configurations.Roles;
using Sergin.Administration.UserAccess.Application.Configurations.Scopes;
using Sergin.Administration.UserAccess.Application.Contracts.Roles.Commands.Create;
using Sergin.Administration.UserAccess.Domain.Roles;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Roles.Commands.Create;

internal sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator(IScopeRepository scopes, IRoleQueryRepository roles, PermissionCatalog catalog)
    {
        RuleFor(x => x.OwnerScopeId).NotEmpty();

        RuleFor(x => x.OwnerScopeId)
            // The command carries the scope id untyped, so the repository's ExistsAsync is called through a ScopeId.
            .MustExistIn((id, cancellationToken) => scopes.ExistsAsync(new ScopeId(id), cancellationToken), ScopeTexts.Singular)
            .When(x => x.OwnerScopeId != Guid.Empty);

        RuleFor(x => x.Name.Value)
            .NotEmpty()
            .MaximumLength(RoleName.MaxLength)
            .OverridePropertyName(nameof(CreateRoleCommand.Name));

        RuleFor(x => x.Permissions).NotNull();

        RuleFor(x => x.Permissions)
            .Must(permissions => permissions.All(permission => catalog.TryGet(permission, out _)))
            .WithMessage(_ => ValidationTexts.Template(RoleTexts.UnknownPermissions))
            .When(x => x.Permissions is not null);

        // BR-4. Advisory: ix_roles_owner_scope_id_name guarantees the owner-scope part under a race.
        RuleFor(x => x.Name)
            .MustAsync(async (command, name, cancellationToken) =>
                !await roles.IsNameTakenInLineAsync(command.OwnerScopeId, name.Value, excludeRoleId: null, cancellationToken))
            .WithMessage(_ => ValidationTexts.Template(RoleTexts.NameTakenInLine))
            .When(x => x.OwnerScopeId != Guid.Empty && !string.IsNullOrWhiteSpace(x.Name.Value));
    }
}
