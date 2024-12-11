using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public class PermissionCheck : IPermissionCheck
{
    public Task<TResponse> CheckPermissionAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken) where TResponse : Result
            => Task.FromResult((TResponse)new Result(true));
}