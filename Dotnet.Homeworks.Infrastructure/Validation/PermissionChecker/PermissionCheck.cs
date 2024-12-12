using System.Reflection;
using Dotnet.Homeworks.Infrastructure.Utils;
using Dotnet.Homeworks.Infrastructure.Validation.RequestTypes;
using Dotnet.Homeworks.Shared.Dto;

namespace Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker;

public class PermissionCheck : IPermissionCheck
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionCheck(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<TResponse> CheckPermissionAsync<TRequest, TResponse>(TRequest request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }
        
        var requestType = request.GetType();
        
        if (!requestType
                .GetInterfaces()
                .Any(x =>
                    x == typeof(IClientRequest) ||
                    x == typeof(IAdminRequest)))
        {
            return ResultFactory.CreateResult<TResponse>(true);
        }

        var permissionCheckIface = GetPermissionCheckIfaceType(requestType);
        
        var permissionCheckService = _serviceProvider.GetRequiredService(permissionCheckIface);
        
        var permCheckType = permissionCheckService.GetType();
        var permResult = await (Task<PermissionResult>)permCheckType.GetMethod("CheckPermission")!.Invoke(permissionCheckService,
            new object[] { request, cancellationToken })!;
        
        return permResult.IsSuccess 
            ? ResultFactory.CreateResult<TResponse>(true) 
            : ResultFactory.CreateResult<TResponse>(false, error: "Not enough permisssions");
    }

    private static Type GetPermissionCheckIfaceType(Type requestType)
    {
        var ifaces = new[]
        {
            requestType.GetInterface(nameof(IClientRequest)),
            requestType.GetInterface(nameof(IAdminRequest))
        };

        return typeof(IPermissionCheck<>)
            .MakeGenericType(ifaces.First(x => x is not null)
                             ?? throw new InvalidOperationException(
                                 "Request doesn't implement any interface to check permission"));
    }

    public static List<(Type Iface, Type Impl)> GetPermissionChecksFrom(params Assembly[] assemblies)
    {
        Func<Type, bool> isPermissionCheckInterface = t =>
            t.IsGenericType &&
            t.GetGenericTypeDefinition() == typeof(IPermissionCheck<>);

        var checks = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(x => x
                .GetInterfaces()
                .Any(isPermissionCheckInterface));

        return checks
            .Select(x => (
                Iface: x
                    .GetInterfaces()
                    .First(isPermissionCheckInterface),
                Impl: x))
            .ToList();
    }
}