using OpenTelemetry.Trace;
using Dotnet.Homeworks.MainProject.Configuration;
using OpenTelemetry.Metrics;

namespace Dotnet.Homeworks.MainProject.ServicesExtensions.OpenTelemetry;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOpenTelemetry(this IServiceCollection services,
        OpenTelemetryConfig openTelemetryConfiguration)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder => builder
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(otlp => otlp.Endpoint = new Uri(openTelemetryConfiguration.OtlpExporterEndpoint))
                .AddHttpClientInstrumentation()
                .AddJaegerExporter())
            .WithMetrics(builder => builder
                .AddMeter("Dotnet.Homeworks.Metrics")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter());

        return services;
    }
}