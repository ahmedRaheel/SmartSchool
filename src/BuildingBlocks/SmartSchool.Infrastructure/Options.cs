namespace SmartSchool.Infrastructure.Options;

public sealed class DatabaseOptions
{
    public const string SectionName = "Database";

    public string ConnectionString { get; init; } = string.Empty;

    public int CommandTimeoutSeconds { get; init; } = 30;
}

public sealed class IdentityOptions
{
    public const string SectionName = "Identity";

    public string Authority { get; init; } = string.Empty;

    public string Audience { get; init; } = "smartschool-api";

    public bool RequireHttpsMetadata { get; init; } = true;
}

public sealed class KafkaOptions
{
    public const string SectionName = "Kafka";

    public bool Enabled { get; init; } = true;

    public string BootstrapServers { get; init; } = "localhost:9092";

    public string ClientId { get; init; } = "smartschool-api";

    public string GroupId { get; init; } = "smartschool";
}

public sealed class HangfireOptions
{
    public const string SectionName = "Hangfire";

    public bool Enabled { get; init; } = true;

    public string DashboardPath { get; init; } = "/ops/jobs";
}

public sealed class AiOptions
{
    public const string SectionName = "AI";

    public string Provider { get; init; } = "OpenAI";

    public string ChatModel { get; init; } = string.Empty;

    public string EmbeddingModel { get; init; } = string.Empty;

    public string ApiKey { get; init; } = string.Empty;

    public double Temperature { get; init; } = 0.2;
}

public sealed class MachineLearningOptions
{
    public const string SectionName = "ML";

    public string BaseUrl { get; init; } = "http://localhost:8000";

    public int TimeoutSeconds { get; init; } = 15;
}
