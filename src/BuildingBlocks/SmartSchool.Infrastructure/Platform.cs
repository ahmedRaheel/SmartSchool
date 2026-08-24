using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Confluent.Kafka;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Messaging;
using SmartSchool.Infrastructure.Persistence;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL.ColumnWriters;
using NpgsqlTypes;
using SmartSchool.Infrastructure.Errors;
using SmartSchool.Infrastructure.Options;
using SmartSchool.SharedKernel.Constants;
using System.Text.Json;
using SmartSchool.Infrastructure.DependencyInjection;
using Serilog.Sinks.PostgreSQL;

namespace SmartSchool.Infrastructure;

public sealed class KafkaPublisher(
	IOptionsMonitor<KafkaOptions> options) : IIntegrationEventPublisher
{
	public async Task PublishAsync<T>(
		string topic,
		T value,
		CancellationToken cancellationToken)
	{
		var currentOptions = options.CurrentValue;

		if (!currentOptions.Enabled)
		{
			return;
		}

		var producerConfig = new ProducerConfig
		{
			BootstrapServers = currentOptions.BootstrapServers,
			ClientId = currentOptions.ClientId,
			EnableIdempotence = true,
			Acks = Acks.All
		};

		using var producer =
			new ProducerBuilder<string, string>(producerConfig)
				.Build();

		var message = new Message<string, string>
		{
			Key = Guid.NewGuid().ToString("N"),
			Value = JsonSerializer.Serialize(value)
		};

		await producer.ProduceAsync(
			topic,
			message,
			cancellationToken);
	}
}

public static class PlatformRegistration
{
	public static WebApplicationBuilder AddSmartSchoolPlatform(
		this WebApplicationBuilder builder)
	{
		ConfigureOptions(builder.Services, builder.Configuration);
		ConfigureLogging(builder);
		ConfigureExceptionHandling(builder.Services);
		ConfigureHangfire(builder.Services, builder.Configuration);
		builder.Services.AddSmartSchoolDataPlatform(builder.Configuration);

		builder.Services.AddSingleton<KafkaPublisher>();
		builder.Services.AddSingleton<IIntegrationEventPublisher>(sp => sp.GetRequiredService<KafkaPublisher>());

		return builder;
	}

	public static IApplicationBuilder UseCorrelationId(
		this IApplicationBuilder application)
	{
		return application.Use(
			async (context, next) =>
			{
				var correlationId =
					context.Request.Headers[ApiRoutes.CorrelationHeader].FirstOrDefault()
					?? Guid.NewGuid().ToString("N");

				var traceId = Activity.Current?.TraceId.ToString()
					?? context.TraceIdentifier;

				context.Response.OnStarting(() =>
				{
					context.Response.Headers[ApiRoutes.CorrelationHeader] = correlationId;
					context.Response.Headers[ApiRoutes.TraceHeader] = traceId;
					return Task.CompletedTask;
				});

				using (LogContext.PushProperty(
					"CorrelationId",
					correlationId))
				using (LogContext.PushProperty(
					"TraceId",
					traceId))
				{
					await next();
				}
			});
	}

	private static void ConfigureLogging(
		WebApplicationBuilder builder)
	{
		builder.Host.UseSerilog(
			(context, services, loggerConfiguration) =>
			{
				var options = services
					.GetRequiredService<IOptions<LoggingOptions>>()
					.Value;

				loggerConfiguration
					.ReadFrom.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.Enrich.WithMachineName()
					.Enrich.WithEnvironmentName()
					.Enrich.WithThreadId()
					.Enrich.WithProperty("ApplicationEntity", ApplicationConstants.ApplicationName)
					.Enrich.WithProperty("Service", ApplicationConstants.ApplicationName);

				if (options.ConsoleEnabled)
				{
					loggerConfiguration.WriteTo.Console();
				}

				if (options.FileEnabled)
				{
					Directory.CreateDirectory(options.LogDirectory);
					loggerConfiguration.WriteTo.File(
						Path.Combine(options.LogDirectory, "smartschool-.log"),
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
						$"Logging database connection string '{options.DatabaseConnectionStringName}' is required when database logging is enabled.");
				}

				if (!Enum.TryParse<LogEventLevel>(options.DatabaseMinimumLevel, true, out var minimumLevel))
				{
					minimumLevel = LogEventLevel.Information;
				}

				IDictionary<string, ColumnWriterBase> columnWriters =
					new Dictionary<string, ColumnWriterBase>
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

				loggerConfiguration.WriteTo.PostgreSQL(
					connectionString,
					options.DatabaseTable,
					columnWriters,
					restrictedToMinimumLevel: minimumLevel,
					schemaName: options.DatabaseSchema,
					needAutoCreateTable: false);
			});
	}

	private static void ConfigureOptions(
		IServiceCollection services,
		IConfiguration configuration)
	{
		services
			.AddOptions<DatabaseOptions>()
			.Bind(configuration.GetSection(DatabaseOptions.SectionName))
			.Validate(
				options => !string.IsNullOrWhiteSpace(
					options.ConnectionString),
				"Database connection string is required.")
			.ValidateOnStart();

		services
			.AddOptions<AuthenticationOptions>()
			.Bind(configuration.GetSection(AuthenticationOptions.SectionName))
			.Validate(
				options => !string.IsNullOrWhiteSpace(
					options.Authority),
				"Identity authority is required.")
			.ValidateOnStart();

		services
			.AddOptions<KafkaOptions>()
			.Bind(configuration.GetSection(KafkaOptions.SectionName));

		services
			.AddOptions<HangfireOptions>()
			.Bind(configuration.GetSection(HangfireOptions.SectionName));

		services
			.AddOptions<AiOptions>()
			.Bind(configuration.GetSection(AiOptions.SectionName));

		services
			.AddOptions<MachineLearningOptions>()
			.Bind(configuration.GetSection(
				MachineLearningOptions.SectionName));

		services
			.AddOptions<ErrorHandlingOptions>()
			.Bind(configuration.GetSection(ErrorHandlingOptions.SectionName))
			.Validate(options => Uri.TryCreate(options.InternalServerErrorTypeUri, UriKind.Absolute, out _),
				"ErrorHandling:InternalServerErrorTypeUri must be an absolute URI.")
			.ValidateOnStart();

		services
			.AddOptions<LoggingOptions>()
			.Bind(configuration.GetSection(LoggingOptions.SectionName))
			.Validate(options => !options.DatabaseEnabled || !string.IsNullOrWhiteSpace(options.DatabaseConnectionStringName),
				"Logging database connection string name is required when database logging is enabled.")
			.Validate(options => !options.DatabaseEnabled || !string.IsNullOrWhiteSpace(options.DatabaseSchema),
				"Logging database schema is required when database logging is enabled.")
			.Validate(options => !options.DatabaseEnabled || !string.IsNullOrWhiteSpace(options.DatabaseTable),
				"Logging database table is required when database logging is enabled.")
			.ValidateOnStart();
	}

	private static void ConfigureExceptionHandling(
		IServiceCollection services)
	{
		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();
	}


	private static void ConfigureHangfire(
		IServiceCollection services,
		IConfiguration configuration)
	{
		var databaseOptions =
			configuration
				.GetSection(DatabaseOptions.SectionName)
				.Get<DatabaseOptions>();

		var hangfireOptions = configuration
			.GetSection(HangfireOptions.SectionName)
			.Get<HangfireOptions>();

		if (hangfireOptions?.Enabled == false ||
			string.IsNullOrWhiteSpace(databaseOptions?.ConnectionString) ||
			databaseOptions.ConnectionString.StartsWith("InMemory:", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		services.AddHangfire(
			hangfire =>
			{
				hangfire.UsePostgreSqlStorage(
					storage =>
					{
						storage.UseNpgsqlConnection(
							databaseOptions.ConnectionString);
					});
			});

		services.AddHangfireServer();
	}
}
