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
    
    public static IServiceCollection AddPipelineBehaviorsForFeaturesNamespace(
        this IServiceCollection services, 
        string @namespace,  
        Assembly namespaceAssembly, 
        Assembly pipelineBehaviorsAssembly)
    {
        var pipes = PipelineBehaviorFinder.FindPipelineBehaviorsInNamespace(
            @namespace, 
            namespaceAssembly, 
            pipelineBehaviorsAssembly);

        foreach (var (iface, impl) in pipes)
        {
            services.Add(new ServiceDescriptor(iface, impl, ServiceLifetime.Transient));
        }

        return services;
    }

    private static IServiceCollection AddHandlers(this IServiceCollection services, 
        IEnumerable<(Type, Type)> handlers, 
        ServiceLifetime lifetime)
    {
        foreach (var (iface, impl) in handlers)
        {
            services.Add(new ServiceDescriptor(iface, impl, lifetime));
        }

        return services;
    }
}