using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Mediator;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.Behaviors;

public class PermissionCheckPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IPermissionCheck _permissionCheck;

    public PermissionCheckPipelineBehavior(IPermissionCheck permissionCheck)
    {
        _permissionCheck = permissionCheck;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var res = await _permissionCheck.CheckPermissionAsync<TRequest, TResponse>(request, cancellationToken);
        if (res is Result { IsSuccess: true })
        {
            return await next();
        }

        return res;
    }
}