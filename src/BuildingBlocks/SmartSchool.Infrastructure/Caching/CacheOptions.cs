namespace SmartSchool.Infrastructure.Caching;

/// <summary>Defines supported distributed cache providers.</summary>
public enum DistributedCacheProvider
{
    /// <summary>Uses PostgreSQL as the distributed L2 cache.</summary>
    PostgreSql,

    /// <summary>Uses Redis as the distributed L2 cache.</summary>
    Redis,

    /// <summary>Uses process memory only.</summary>
    Memory
}

/// <summary>Configures HybridCache and its distributed L2 provider.</summary>
public sealed class CacheOptions
{
    /// <summary>Gets the configuration section name.</summary>
    public const string SectionName = "Cache";

    /// <summary>Gets or sets the distributed cache provider.</summary>
    public DistributedCacheProvider Provider { get; set; } =
        DistributedCacheProvider.PostgreSql;

    /// <summary>Gets or sets the default cache lifetime in minutes.</summary>
    public int DefaultExpirationMinutes { get; set; } = 30;

    /// <summary>Gets or sets the PostgreSQL cache schema.</summary>
    public string PostgreSqlSchema { get; set; } = "Infrastructure";

    /// <summary>Gets or sets the PostgreSQL cache table.</summary>
    public string PostgreSqlTable { get; set; } = "DistributedCache";
	public string RedisConnectionStringName { get; set; } = 	"Redis";
	public string InstanceName { get; set; } = 	"SmartSchool:";
}
