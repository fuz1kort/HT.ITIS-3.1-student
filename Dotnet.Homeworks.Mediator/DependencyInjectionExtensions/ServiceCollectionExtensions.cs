using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] handlersAssemblies)
    {
        if (handlersAssemblies == null || handlersAssemblies.Length == 0)
        {
            throw new ArgumentException("At least one assembly must be specified.");
        }

        var handlerTypes = handlersAssemblies.SelectMany(a => a.GetTypes()).ToList();

        var handlers = handlerTypes
            .Where(t =>
                        t.GetInterfaces().Any(i => i.IsGenericType &&
                                                   i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
            .ToList();

        foreach (var handlerType in handlers)
        {
            foreach (var implementedInterface in handlerType.GetInterfaces())
            {
                if (implementedInterface.IsGenericType &&
                    implementedInterface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                {
                    services.AddTransient(implementedInterface, handlerType);
                }
            }
        }

        services.AddSingleton<IMediator, Mediator>();
        return services;
    }
}