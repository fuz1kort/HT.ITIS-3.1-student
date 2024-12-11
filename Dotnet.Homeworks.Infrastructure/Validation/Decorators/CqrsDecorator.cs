using Dotnet.Homeworks.Mediator;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class CqrsDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, TResponse
{
    protected CqrsDecorator() : base()
    {
    }

    public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        =>  await base.Handle(request, cancellationToken); 
}