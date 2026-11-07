using Sergin.SharedKernel.Application.Aggregates;
using Sergin.Administration.UserAccess.Domain.Scopes;

namespace Sergin.Administration.UserAccess.Application.Configurations.Scopes;

internal sealed class ScopeAggregateFeatureConfiguration : IAggregateFeatureConfiguration<Scope>
{
    public void Configure(AggregateFeatureBuilder<Scope> builder) => builder.Audited().SoftDeletable().Versioned()
            .Permissions(ScopePermissions.Read, ScopePermissions.Manage)
            .Texts(ScopeTexts.Category)
            .Named(ScopeTexts.Singular);
}
