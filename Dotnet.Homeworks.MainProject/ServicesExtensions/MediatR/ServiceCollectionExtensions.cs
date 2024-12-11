using Dotnet.Homeworks.Mediator.DependencyInjectionExtensions;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.MediatR;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediator();

        return services;
    }
}