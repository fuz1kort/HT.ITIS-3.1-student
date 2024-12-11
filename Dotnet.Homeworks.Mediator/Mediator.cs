using Microsoft.Extensions.DependencyInjection;

namespace Dotnet.Homeworks.Mediator;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));
        
        var service = _serviceProvider.GetRequiredService(handlerType);
        return (Task<TResponse>)service.GetType().GetMethod("Handle")!.Invoke(service, new object[] { request, cancellationToken })!;
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
}