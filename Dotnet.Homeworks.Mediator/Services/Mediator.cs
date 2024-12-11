namespace Dotnet.Homeworks.Mediator.Services;

public class Mediator: IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();
        var handlerType = typeof(IRequestHandler<IRequest<TResponse>, TResponse>);
        var myService = _serviceProvider.GetService(handlerType);
        
        if (myService == null)
        {
            throw new InvalidOperationException($"Handler not found for {requestType}");
        }
        
        if (_serviceProvider.GetService(typeof(IRequestHandler<IRequest<TResponse>, TResponse>)) 
            is not IRequestHandler<IRequest<TResponse>, TResponse> service)
            throw new InvalidOperationException();
        
        return await service.Handle(request, cancellationToken);
    }

    public async Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
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