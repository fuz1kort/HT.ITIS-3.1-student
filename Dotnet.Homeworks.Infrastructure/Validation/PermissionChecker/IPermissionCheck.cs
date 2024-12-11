using Dotnet.Homeworks.Infrastructure.Utils;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public interface IPermissionCheck
{
    Task<TResponse> CheckPermissionAsync<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken);
}