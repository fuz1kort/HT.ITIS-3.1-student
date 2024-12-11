using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public class PermissionCheck : IPermissionCheck
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<IEnumerable<PermissionResult>> CheckPermissionAsync<TRequest>(TRequest request)
    {
        var permissionResults = new List<PermissionResult>();

        var user = _httpContextAccessor.HttpContext?.User;
        PermissionResult permissionResult;
        if (user == null || !user.Identity!.IsAuthenticated)
        {
            permissionResult = new PermissionResult(false, "User is not authenticated");
            permissionResults.Add(permissionResult);
            return Task.FromResult<IEnumerable<PermissionResult>>(permissionResults);
        }

        var requestType = request!.GetType();

        if (requestType == typeof(IClientRequest)
            && user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value.Equals(Roles.User.ToString())
            || requestType == typeof(IAdminRequest)
            && user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)!.Value.Equals(Roles.Admin.ToString()))
        {
            permissionResult = new PermissionResult(true, "Access granted");
            permissionResults.Add(permissionResult);
            return Task.FromResult<IEnumerable<PermissionResult>>(permissionResults);
        }

        permissionResult = new PermissionResult(false, "Access denied");
        permissionResults.Add(permissionResult);
        return Task.FromResult<IEnumerable<PermissionResult>>(permissionResults);
    }
}