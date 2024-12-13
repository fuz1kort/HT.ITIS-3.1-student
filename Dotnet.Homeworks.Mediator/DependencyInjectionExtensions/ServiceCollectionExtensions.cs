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

        foreach (var handler in handlers)
        {
            foreach (var implementedInterface in handler.GetInterfaces())
            {
                if (implementedInterface.IsGenericType &&
                    implementedInterface.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
                {
                    services.AddTransient(implementedInterface, handler);
                }
            }
        }

        services.AddSingleton<IMediator, Mediator>();
        return services;
    }
    
    public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services, 
        string @namespace,  
        Assembly namespaceAssembly, 
        Assembly pipelineBehaviorsAssembly)
    {
        var pipelineBehaviorTypes = pipelineBehaviorsAssembly.GetTypes();
        
        var pipelineBehaviors = pipelineBehaviorTypes
            .Where(t =>
                t.GetInterfaces().Any(i => i.IsGenericType &&
                                           i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>)))
            .ToList();

        foreach (var pipelineBehavior in pipelineBehaviors)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), pipelineBehavior);
        }
        
        return services;
    }
}