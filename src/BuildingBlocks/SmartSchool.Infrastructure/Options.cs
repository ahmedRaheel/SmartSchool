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
}

public sealed class KafkaOptions
{
	public const string SectionName = ConfigurationSections.Kafka;

	public bool Enabled { get; init; } = true;

	public string BootstrapServers { get; init; } = "localhost:9092";

	public string ClientId { get; init; } = "smartschool-api";

	public string GroupId { get; init; } = "smartschool";
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

	public string BaseUrl { get; init; } = "http://localhost:8000";

	public int TimeoutSeconds { get; init; } = 15;
}
