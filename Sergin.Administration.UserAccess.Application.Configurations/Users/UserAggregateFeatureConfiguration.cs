using Sergin.SharedKernel.Application.Aggregates;
using Sergin.Administration.UserAccess.Domain.Users;

namespace Sergin.Administration.UserAccess.Application.Configurations.Users;

internal sealed class UserAggregateFeatureConfiguration : IAggregateFeatureConfiguration<User>
{
    public void Configure(AggregateFeatureBuilder<User> builder) =>
        builder.Permissions(UserPermissions.Read, UserPermissions.Manage, UserPermissions.AssignmentsManage)
            .Named(UserTexts.Singular);
}
