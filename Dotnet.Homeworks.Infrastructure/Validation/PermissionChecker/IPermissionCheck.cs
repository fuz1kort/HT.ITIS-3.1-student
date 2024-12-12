using Dotnet.Homeworks.Infrastructure.Utils;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public interface IPermissionCheck
{
    Task<TResponse> CheckPermissionAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken);
}

public interface IPermissionCheck<in TRequest>
{
    Task<PermissionResult> CheckPermission(TRequest request, CancellationToken cancellationToken);
}