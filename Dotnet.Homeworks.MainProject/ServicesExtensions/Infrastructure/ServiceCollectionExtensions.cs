using Dotnet.Homeworks.Infrastructure.Validation.PermissionChecker.DependencyInjectionExtensions;
using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;
using FluentValidation;
using AssemblyReference = Dotnet.Homeworks.Infrastructure.Helpers.AssemblyReference;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddPermissionChecks(AssemblyReference.Assembly);
        services.AddMediator();
        services.AddPipelineBehaviors(Features.UserManagement.DirectoryReference.Namespace,
            Helpers.AssemblyReference.Assembly, 
            AssemblyReference.Assembly);

        return services;
    }
}