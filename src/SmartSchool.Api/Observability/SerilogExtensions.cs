using Serilog;
using Serilog.Events;

namespace SmartSchool.Api.Observability;

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSmartSchoolSerilog(
        this WebApplicationBuilder builder,
        string serviceName)
    {
        var logDirectory = builder.Configuration["Serilog:LogDirectory"] ?? "logs";
        Directory.CreateDirectory(logDirectory);

        builder.Host.UseSerilog((context, services, logger) =>
        {
            logger
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(
                    Path.Combine(logDirectory, $"{serviceName}-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 30,
                    shared: true);
        });

        return builder;
    }
}
