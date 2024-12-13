using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.UserManagement.PermissionChecks;

public class AdminPermissionCheck : IPermissionCheck<IAdminRequest>
{
    private readonly HttpContext _httpContext;

    public AdminPermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public Task<PermissionResult> CheckPermission(IAdminRequest request, CancellationToken cancellationToken)
    {
        var claims = _httpContext.User.Claims;
        return claims.Any(x => x.Type == ClaimTypes.Role && x.Value == Roles.Admin.ToString())
            ? Task.FromResult(new PermissionResult(true))
            : Task.FromResult(new PermissionResult(false, "Don't have permission"));
    }
}