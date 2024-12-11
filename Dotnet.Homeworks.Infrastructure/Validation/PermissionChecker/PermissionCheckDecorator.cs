using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public class PermissionCheckDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPermissionCheck _permissionCheck;

    protected PermissionCheckDecorator(IPermissionCheck permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        var res = await _permissionCheck.CheckPermissionAsync<TRequest, TResponse>(request, cancellationToken);
        return res;
    }
}