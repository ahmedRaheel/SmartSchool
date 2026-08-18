namespace SmartSchool.Infrastructure.Caching;

/// <summary>
/// Configures the second-level cache used by HybridCache.
/// </summary>
public sealed class CacheOptions
{
	public const string SectionName = "Caching";

	public CacheProvider Provider { get; init; } = CacheProvider.Memory;

	public string RedisConnectionStringName { get; init; } = "Redis";

	public string InstanceName { get; init; } = "SmartSchool:";

	public int DefaultExpirationMinutes { get; init; } = 10;
}

public enum CacheProvider
{
	Memory,
	Redis
}
