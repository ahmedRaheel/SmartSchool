using Microsoft.Extensions.Options;
using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using Serilog.Sinks.PostgreSQL.ColumnWriters;

namespace SmartSchool.Identity.Api.Observability;

public sealed class LoggingOptions
{
    public const string SectionName = "LoggingOptions";
    public bool ConsoleEnabled { get; init; }
    public bool FileEnabled { get; init; }
    public string LogDirectory { get; init; } = string.Empty;
    public int RetainedFileCountLimit { get; init; }
    public bool DatabaseEnabled { get; init; }
    public string DatabaseConnectionStringName { get; init; } = string.Empty;
    public string DatabaseSchema { get; init; } = string.Empty;
    public string DatabaseTable { get; init; } = string.Empty;
    public string DatabaseMinimumLevel { get; init; } = string.Empty;
}

public static class SerilogExtensions
{
    public static WebApplicationBuilder AddSmartSchoolSerilog(
        this WebApplicationBuilder builder,
        string serviceName)
    {
        builder.Services
            .AddOptions<LoggingOptions>()
            .Bind(builder.Configuration.GetSection(LoggingOptions.SectionName))
            .Validate(options => !options.FileEnabled || !string.IsNullOrWhiteSpace(options.LogDirectory), "LoggingOptions:LogDirectory is required when file logging is enabled.")
            .Validate(options => !options.DatabaseEnabled || !string.IsNullOrWhiteSpace(options.DatabaseConnectionStringName), "LoggingOptions:DatabaseConnectionStringName is required when database logging is enabled.")
            .Validate(options => !options.DatabaseEnabled || !string.IsNullOrWhiteSpace(options.DatabaseSchema), "LoggingOptions:DatabaseSchema is required when database logging is enabled.")
            .Validate(options => !options.DatabaseEnabled || !string.IsNullOrWhiteSpace(options.DatabaseTable), "LoggingOptions:DatabaseTable is required when database logging is enabled.")
            .ValidateOnStart();

        builder.Host.UseSerilog((context, services, logger) =>
        {
            var options = services.GetRequiredService<IOptions<LoggingOptions>>().Value;

            logger
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Service", serviceName)
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning);

            if (options.ConsoleEnabled)
            {
                logger.WriteTo.Console();
            }

            if (options.FileEnabled)
            {
                Directory.CreateDirectory(options.LogDirectory);
                logger.WriteTo.File(
                    Path.Combine(options.LogDirectory, $"{serviceName}-.log"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: options.RetainedFileCountLimit,
                    shared: true);
            }

            if (!options.DatabaseEnabled)
            {
                return;
            }

            var connectionString = context.Configuration
                .GetConnectionString(options.DatabaseConnectionStringName);

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    $"Logging database connection string '{options.DatabaseConnectionStringName}' is required.");
            }

            if (!Enum.TryParse<LogEventLevel>(options.DatabaseMinimumLevel, true, out var minimumLevel))
            {
                minimumLevel = LogEventLevel.Information;
            }

            IDictionary<string, ColumnWriterBase> columnWriters = new Dictionary<string, ColumnWriterBase>
            {
                ["timestamp_utc"] = new TimestampColumnWriter(NpgsqlDbType.TimestampTz),
                ["level"] = new LevelColumnWriter(true, NpgsqlDbType.Varchar),
                ["service"] = new SinglePropertyColumnWriter("Service", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l"),
                ["message"] = new RenderedMessageColumnWriter(NpgsqlDbType.Text),
                ["message_template"] = new MessageTemplateColumnWriter(NpgsqlDbType.Text),
                ["exception"] = new ExceptionColumnWriter(NpgsqlDbType.Text),
                ["trace_id"] = new SinglePropertyColumnWriter("TraceId", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l"),
                ["correlation_id"] = new SinglePropertyColumnWriter("CorrelationId", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l"),
                ["request_path"] = new SinglePropertyColumnWriter("RequestPath", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l"),
                ["properties"] = new PropertiesColumnWriter(NpgsqlDbType.Jsonb)
            };

            logger.WriteTo.PostgreSQL(
                connectionString,
                options.DatabaseTable,
                columnWriters,
                restrictedToMinimumLevel: minimumLevel,
                schemaName: options.DatabaseSchema,
                needAutoCreateTable: false);
        });

        return builder;
    }
}
