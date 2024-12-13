using System.Reflection;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    // public static void AddPermissionChecks(
    //     this IServiceCollection serviceCollection,
    //     Assembly assembly
    // )
    // {
    // }

    public static void AddPermissionChecks(
        this IServiceCollection serviceCollection,
        params Assembly[] assemblies
    )
    {
        serviceCollection.AddScoped<IPermissionCheck, PermissionCheck>();
        serviceCollection.AddHttpContextAccessor();
        var tuples = PermissionCheck.GetPermissionChecksFrom(assemblies);
        tuples.ForEach(tuple =>
        {
            serviceCollection.AddScoped(tuple.Iface, tuple.Impl);
        });

        serviceCollection.AddScoped<IPermissionCheck, PermissionCheck>();
    }
}