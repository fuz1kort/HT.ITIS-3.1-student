using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        
        var service = _serviceProvider.GetRequiredService(handlerType);
        return await ExecutePipelineBehaviors(
            request,
            () => (Task<TResponse>)service.GetType().GetMethod("Handle")!.Invoke(service, new object[] { request, cancellationToken })!,
            cancellationToken);
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<>).MakeGenericType(requestType);
        
        var service = _serviceProvider.GetRequiredService(handlerType);
        return Task.FromResult((Task)service.GetType().GetMethod("Handle")!.Invoke(service, new object[] { request, cancellationToken })!);
    }

    public async Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        if (_serviceProvider.GetService(typeof(IRequestHandler<IRequest<dynamic>, dynamic>))
            is not IRequestHandler<IRequest<dynamic>, dynamic> service)
            return null;

        return await service.Handle(request, cancellationToken);
    }

    private async Task<TResponse> ExecutePipelineBehaviors<TRequest, TResponse>(
        TRequest request,
        RequestHandlerDelegate<TResponse> handler,
        CancellationToken cancellationToken = default)
    {
        var behaviors = _serviceProvider
            .GetServices<IPipelineBehavior<TRequest, TResponse>>()
            .ToList();

        if (behaviors.Count == 0)
        {
            return await handler.Invoke();
        }

        var res = await Pipe(request, behaviors, 0, handler, cancellationToken);
        return res;
    }
    
    private static async Task<TResponse> Pipe<TRequest, TResponse>(
        TRequest request,
        List<IPipelineBehavior<TRequest, TResponse>> behaviors,
        int index, 
        RequestHandlerDelegate<TResponse> lastHandler,
        CancellationToken cancellationToken = default)
    {
        if (behaviors.Count - 1 == index)
        {
            return await behaviors[index].Handle(request, lastHandler, cancellationToken); 
        }

        return await behaviors[index].Handle(
            request, 
            () => Pipe(request, behaviors, index + 1, lastHandler, cancellationToken), 
            cancellationToken);
    }
}