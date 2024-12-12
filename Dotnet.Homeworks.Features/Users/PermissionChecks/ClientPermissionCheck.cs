using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.Users.PermissionChecks;

public class ClientPermissionCheck : IPermissionCheck<IClientRequest>
{
    private readonly HttpContext _httpContext;

    public ClientPermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public Task<TResponse> CheckPermission<TResponse>(IClientRequest request, CancellationToken cancellationToken)
        where TResponse : Result
    {
        var claims = _httpContext.User.Claims;
        return claims.Any(claim => claim.Type == ClaimTypes.NameIdentifier && claim.Value == request.Guid.ToString())
            ? Task.FromResult((TResponse)new Result(true))
            : Task.FromResult((TResponse)new Result(false, "Don't have permission"));
    }
}