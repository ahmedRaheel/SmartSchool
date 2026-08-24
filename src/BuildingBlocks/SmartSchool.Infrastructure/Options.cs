using SmartSchool.Infrastructure.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Options;

public sealed class DatabaseOptions
{
	public const string SectionName = ConfigurationSections.Database;

	public string ConnectionString { get; init; } = string.Empty;

	public int CommandTimeoutSeconds { get; init; } = 30;
}

public sealed class AuthenticationOptions
{
	public const string SectionName = ConfigurationSections.Identity;

	public string Authority { get; init; } = string.Empty;

	public string Audience { get; init; } = AuthenticationConstants.DefaultAudience;

	public bool RequireHttpsMetadata { get; init; } = true;

	public string MetadataAddress { get; init; } = string.Empty;

	public string ValidIssuer { get; init; } = string.Empty;
	public IdentityProvider Provider { get; init; } = IdentityProvider.IdentityServer;
}

public sealed class KafkaOptions
{
	public const string SectionName = ConfigurationSections.Kafka;

	public bool Enabled { get; init; } = true;

	public string BootstrapServers { get; init; } = string.Empty;

	public string ClientId { get; init; } = string.Empty;

	public string GroupId { get; init; } = string.Empty;
}

public sealed class HangfireOptions
{
	public const string SectionName = ConfigurationSections.Hangfire;

	public bool Enabled { get; init; } = true;

	public string DashboardPath { get; init; } = ApiRoutes.OperationsJobs;
}

public sealed class AiOptions
{
	public const string SectionName = ConfigurationSections.ArtificialIntelligence;

	public string Provider { get; init; } = "OpenAI";

	public string ChatModel { get; init; } = string.Empty;

	public string EmbeddingModel { get; init; } = string.Empty;

	public string ApiKey { get; init; } = string.Empty;

	public double Temperature { get; init; } = 0.2;
}

public sealed class MachineLearningOptions
{
	public const string SectionName = ConfigurationSections.MachineLearning;

	public string BaseUrl { get; init; } = string.Empty;

	public int TimeoutSeconds { get; init; } = 15;
}


public sealed class LoggingOptions
{
	public const string SectionName = "LoggingOptions";

	public bool ConsoleEnabled { get; init; } = true;
	public bool FileEnabled { get; init; } = true;
	public string LogDirectory { get; init; } = "logs";
	public int RetainedFileCountLimit { get; init; } = 30;
	public bool DatabaseEnabled { get; init; } = true;
	public string DatabaseConnectionStringName { get; init; } = "SmartSchool";
	public string DatabaseSchema { get; init; } = "observability";
	public string DatabaseTable { get; init; } = "application_log";
	public string DatabaseMinimumLevel { get; init; } = "Information";
}

public sealed class ErrorHandlingOptions
{
	public const string SectionName = "ErrorHandling";
	public string InternalServerErrorTypeUri { get; init; } = string.Empty;
}
