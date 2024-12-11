using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public class PermissionCheck : IPermissionCheck
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<TResponse> CheckPermissionAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken = default)

    {
        var user = _httpContextAccessor.HttpContext?.User;
        PermissionResult permissionResult;
        if (user == null || !user.Identity!.IsAuthenticated)
        {
            permissionResult = new PermissionResult(false, "User is not authenticated");
            return Task.FromResult((Result)permissionResult);
        }

        var requestType = request!.GetType();

        if (requestType == typeof(IClientRequest)
            && user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value.Equals(Roles.User.ToString())
            || requestType == typeof(IAdminRequest)
            && user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value.Equals(Roles.Admin.ToString()))
        {
            permissionResult = new PermissionResult(true, "Access granted");
            return Task.FromResult((TResponse)permissionResult);
        }

        permissionResult = new PermissionResult(false, "Access denied");
        return Task.FromResult((TResponse)permissionResult);
    }
}