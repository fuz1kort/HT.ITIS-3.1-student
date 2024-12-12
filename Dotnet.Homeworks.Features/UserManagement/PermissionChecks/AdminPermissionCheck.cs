using System.Security.Claims;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.Enums;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;
using Microsoft.AspNetCore.Http;

namespace Dotnet.Homeworks.Features.UserManagement.PermissionChecks;

public class AdminPermissionCheck : IPermissionCheck<IAdminRequest>
{
    private readonly HttpContext _httpContext;

    public AdminPermissionCheck(IHttpContextAccessor httpContextAccessor)
    {
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public Task<TResponse> CheckPermission<TResponse>(IAdminRequest request, CancellationToken cancellationToken)
        where TResponse : Result
    {
        var claims = _httpContext.User.Claims;
        return claims.Any(x => x.Type == ClaimTypes.Role && x.Value == Roles.Admin.ToString())
                ? Task.FromResult((TResponse)new Result(true))
                : Task.FromResult((TResponse)new Result(false, "Don't have permission"));
    }
}