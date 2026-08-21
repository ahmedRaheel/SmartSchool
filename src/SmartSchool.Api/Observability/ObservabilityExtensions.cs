using System.Diagnostics;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace SmartSchool.Api.Observability;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddSmartSchoolObservability(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        var endpoint = configuration["OpenTelemetry:OtlpEndpoint"] ?? Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT");
        var telemetry = services.AddOpenTelemetry().ConfigureResource(r => r.AddService(serviceName));
        telemetry.WithTracing(t =>
        {
            t.AddAspNetCoreInstrumentation(o => o.RecordException = true)
             .AddHttpClientInstrumentation();
            if (!string.IsNullOrWhiteSpace(endpoint)) t.AddOtlpExporter(o => o.Endpoint = new Uri(endpoint));
        });
        telemetry.WithMetrics(m =>
        {
            m.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation().AddRuntimeInstrumentation();
            if (!string.IsNullOrWhiteSpace(endpoint)) m.AddOtlpExporter(o => o.Endpoint = new Uri(endpoint));
        });
        return services;
    }

    public static IApplicationBuilder UseTelemetryResponseHeaders(this IApplicationBuilder app) => app.Use(async (context, next) =>
    {
        await next();
        var activity = Activity.Current;
        if (activity is not null) context.Response.Headers["X-Trace-Id"] = activity.TraceId.ToString();
        if (!context.Response.Headers.ContainsKey("X-Correlation-ID"))
            context.Response.Headers["X-Correlation-ID"] = context.TraceIdentifier;
    });
}
