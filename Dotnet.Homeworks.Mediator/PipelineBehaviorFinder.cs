using System.Reflection;

namespace Dotnet.Homeworks.Mediator;

public static class PipelineBehaviorFinder
{
    public static List<(Type Iface, Type Impl)> FindPipelineBehaviorsInNamespace(
        string @namespace, 
        Assembly namespaceAssembly, 
        Assembly pipelineBehaviorsAssembly)
    {
        var types = namespaceAssembly.GetTypes();
        
        Func<Type, bool> isHandlerInterface = i =>
            i.IsGenericType &&
            (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
             i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));

        var requestHandlers = types
            .Where(x => x
                .GetInterfaces()
                .Any(isHandlerInterface))
            .ToList();

        var requestHandlerTypes = requestHandlers
            .Select(x => (
                Iface: x
                    .GetInterfaces()
                    .First(isHandlerInterface),
                Impl: x))
            .ToList();

        var reqAndResps = GetRequestAndResponseTuples(requestHandlerTypes.Select(x => x.Iface));
        var openPipeImpls = GetOpenPipelineBehaviorImpls(pipelineBehaviorsAssembly.GetTypes());

        var result = new List<(Type Iface, Type Impl)>();
        foreach (var (requestType, responseType) in reqAndResps)
        {
            var pipelineIface = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
            var pipelineImpls = openPipeImpls.Select(type => type.MakeGenericType(requestType, responseType));

            result.AddRange(pipelineImpls.Select(pipelineImpl => (pipelineIface, pipelineImpl)));
        }

        return result;
    }

    private static List<Type> GetOpenPipelineBehaviorImpls(IEnumerable<Type> types)
    {
        return types
            .Where(type => type
                .GetInterfaces()
                .Any(i => i.IsGenericType && 
                          i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>)))
            .ToList();
    }

    private static List<(Type RequestType, Type ResponseType)> GetRequestAndResponseTuples(
        IEnumerable<Type> closedRequestHandlerInterfaceTypes)
    {
        return closedRequestHandlerInterfaceTypes
            .Select(x => (RequestType: x.GetGenericArguments()[0], ResponseType: x.GetGenericArguments()[1]))
            .ToList();
    }
}