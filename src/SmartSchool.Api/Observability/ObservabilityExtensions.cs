using System.Diagnostics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SmartSchool.Api.Observability;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddSmartSchoolObservability(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        var endpoint = configuration["OpenTelemetry:OtlpEndpoint"]
            ?? Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");

        var telemetry = services
            .AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName));

        telemetry.WithTracing(tracing =>
        {
            tracing
                .AddAspNetCoreInstrumentation(options => options.RecordException = true)
                .AddHttpClientInstrumentation();

            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                tracing.AddOtlpExporter(options => options.Endpoint = new Uri(endpoint));
            }
        });

        telemetry.WithMetrics(metrics =>
        {
            metrics
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddRuntimeInstrumentation();

            if (!string.IsNullOrWhiteSpace(endpoint))
            {
                metrics.AddOtlpExporter(options => options.Endpoint = new Uri(endpoint));
            }
        });

        return services;
    }

    public static IApplicationBuilder UseTelemetryResponseHeaders(
        this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            var requestCorrelationId =
                context.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            var correlationId = string.IsNullOrWhiteSpace(requestCorrelationId)
                ? context.TraceIdentifier
                : requestCorrelationId;

            // Register before downstream middleware can start the response.
            context.Response.OnStarting(() =>
            {
                var traceId = Activity.Current?.TraceId.ToString();

                context.Response.Headers["X-Correlation-ID"] = correlationId;

                if (!string.IsNullOrWhiteSpace(traceId))
                {
                    context.Response.Headers["X-Trace-Id"] = traceId;
                }

                return Task.CompletedTask;
            });

            await next();
        });
    }
}
