using Confluent.Kafka;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Infrastructure.Persistence;
using Serilog;
using Serilog.Context;
using SmartSchool.Infrastructure.Errors;
using SmartSchool.Infrastructure.Options;
using SmartSchool.SharedKernel.Constants;
using System.Text.Json;

namespace SmartSchool.Infrastructure;

public sealed class KafkaPublisher(
	IOptionsMonitor<KafkaOptions> options)
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
		ConfigureLogging(builder);
		ConfigureOptions(builder.Services, builder.Configuration);
		ConfigureExceptionHandling(builder.Services);
		ConfigureHangfire(builder.Services, builder.Configuration);
		ConfigureMockDatabase(builder.Services);

		builder.Services.AddSingleton<KafkaPublisher>();

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

				context.Response.Headers[ApiRoutes.CorrelationHeader] =
					correlationId;

				using (LogContext.PushProperty(
					"CorrelationId",
					correlationId))
				using (LogContext.PushProperty(
					"TraceId",
					context.TraceIdentifier))
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
				loggerConfiguration
					.ReadFrom.Configuration(context.Configuration)
					.ReadFrom.Services(services)
					.Enrich.FromLogContext()
					.Enrich.WithMachineName()
					.Enrich.WithEnvironmentName()
					.Enrich.WithThreadId()
					.Enrich.WithProperty(
						"ApplicationEntity",
						ApplicationConstants.ApplicationName);
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
			.AddOptions<IdentityOptions>()
			.Bind(configuration.GetSection(IdentityOptions.SectionName))
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
	}

	private static void ConfigureExceptionHandling(
		IServiceCollection services)
	{
		services.AddExceptionHandler<GlobalExceptionHandler>();
		services.AddProblemDetails();
	}

	private static void ConfigureMockDatabase(IServiceCollection services)
	{
		services.AddDbContext<SmartSchoolMockDbContext>(options =>
			options.UseInMemoryDatabase("SmartSchool-Development"));
		services.AddScoped<IEfMockStore, EfMockStore>();
		services.AddScoped<MockDatabaseSeeder>();
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
