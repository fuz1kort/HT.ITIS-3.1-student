using System.Reflection;
using Dotnet.Homeworks.Infrastructure.Utils;
using Mapster;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Mapper;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMappers(this IServiceCollection services, Assembly mapperConfigsAssembly)
    {
        var types = mapperConfigsAssembly.GetTypes();

        var mapperTypes = types.Where(t => typeof(IRegister).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var mapperType in mapperTypes)
        {
            if (Activator.CreateInstance(mapperType) is IRegister mapperInstance)
            {
                mapperInstance.Register(TypeAdapterConfig.GlobalSettings);
            }
        }
        
        var mapperInterfaceTypes = types.Where(t => typeof(IMapper).IsAssignableFrom(t) && t.IsInterface);

        foreach (var interfaceType in mapperInterfaceTypes)
        {
            var mapperImplementations = types.Where(t => interfaceType.IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

            foreach (var implementationType in mapperImplementations)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }

        return services;
    }
}