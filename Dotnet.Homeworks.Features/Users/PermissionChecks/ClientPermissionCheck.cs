using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Users.PermissionChecks;

public class ClientPermissionCheck : IPermissionCheck<IClientRequest>
{
    private readonly HttpContext _httpContext;

    public ClientPermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public Task<PermissionResult> CheckPermission(IClientRequest request, CancellationToken cancellationToken)
    {
        var claims = _httpContext.User.Claims;
        return claims.Any(claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == request.Guid.ToString())
            ? Task.FromResult(new PermissionResult(true))
            : Task.FromResult(new PermissionResult(false, "Don't have permission"));
    }
}