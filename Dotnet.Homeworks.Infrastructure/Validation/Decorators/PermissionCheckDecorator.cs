using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.Decorators;

public class PermissionCheckDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse> 
    where TResponse : Result
{
    private readonly IPermissionCheck _permissionCheck;

    protected PermissionCheckDecorator(IPermissionCheck permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken) 
        => await _permissionCheck.CheckPermissionAsync<TRequest, TResponse>(request, cancellationToken);
}