using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sergin.SharedKernel.Infrastructure.Data.EFCore;
using Sergin.SharedKernel.Modules;
using Sergin.Administration.UserAccess.Application;
using Sergin.Administration.UserAccess.Application.Configurations;
using Sergin.Administration.UserAccess.Application.Contracts;
using Sergin.Administration.UserAccess.Authorization;
using Sergin.Administration.UserAccess.Infrastructure.Data;
using Sergin.Administration.UserAccess.Presentation.Blazor;
using Sergin.Administration.UserAccess.Roles;
using Sergin.Administration.UserAccess.Scopes;
using Sergin.Administration.UserAccess.Users;

namespace Sergin.Administration.UserAccess;

public sealed class UserAccessModule : ISerginWebApiModule, ISerginWebUiModule
{
    public string Name => "UserAccess";

    public string Schema => UserAccessDbContext.Schema;

    public Assembly ApplicationAssembly => UserAccessApplicationAssemblyReference.Assembly;

    public Assembly ContractsAssembly => UserAccessApplicationContractsAssemblyReference.Assembly;

    public Assembly ConfigurationsAssembly => UserAccessApplicationConfigurationsAssemblyReference.Assembly;

    public Assembly UiAssembly => UserAccessBlazorAssemblyReference.Assembly;

    public SerginNavGroup NavGroup => UserAccessNavigation.Group;

    public IReadOnlyCollection<SerginNavItem> NavItems => UserAccessNavigation.Items;

    public void AddServices(IServiceCollection services, IConfigurationSection configuration)
    {
        services.AddModuleDbContext<UserAccessDbContext, IUserAccessDbContext, IUserAccessUnitOfWork>(configuration, UserAccessDbContext.Schema);

        services.AddUserDependencies();

        services.AddRoleDependencies();

        services.AddScopeDependencies();

        services.AddAuthorizationDependencies();
    }

    public Task MigrateAsync(IServiceProvider services) => services.MigrateDbContextAsync<UserAccessDbContext>();

    public void MapEndpoints(RouteGroupBuilder group)
    {
        group.MapUserEndpoints();
        group.MapScopeEndpoints();
        group.MapRoleEndpoints();
    }
}
