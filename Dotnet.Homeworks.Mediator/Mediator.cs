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
        
        // var handler = _serviceProvider.GetService(handlerType);
        var serv = _serviceProvider.GetRequiredService(handlerType);
        return (Task<TResponse>)serv.GetType().GetMethod("Handle")!.Invoke(serv, new object[] { request, cancellationToken })!;
        // var serv = handler as IRequestHandler<IRequest<TResponse>, TResponse>;

        // if(_serviceProvider.GetService(handlerType) 
        //    is not IRequestHandler<IRequest<TResponse>, TResponse> service)
        // {
        //     throw new InvalidOperationException($"No handler registered for {requestType}");
        // }
        

        // return await serv!.Handle(request, cancellationToken);
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest
    {
        if (_serviceProvider.GetService(typeof(IRequestHandler<TRequest>))
            is not IRequestHandler<TRequest> service)
            throw new InvalidOperationException();

        await service.Handle(request, cancellationToken);
    }

    public async Task<dynamic?> Send(dynamic request, CancellationToken cancellationToken = default)
    {
        if (_serviceProvider.GetService(typeof(IRequestHandler<IRequest<dynamic>, dynamic>))
            is not IRequestHandler<IRequest<dynamic>, dynamic> service)
            return null;

        return await service.Handle(request, cancellationToken);
    }
}