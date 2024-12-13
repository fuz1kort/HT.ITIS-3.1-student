using Dotnet.Homeworks.Features.Helpers;
using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using FluentValidation;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddPermissionChecks(AssemblyReference.Assembly);
        services.AddMediator();
        services.AddPipelineBehaviors(Dotnet.Homeworks.Infrastructure.Helpers.AssemblyReference.Assembly);

        return services;
    }
}