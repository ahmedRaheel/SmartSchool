using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartSchool.Infrastructure.Caching;

/// <summary>Registers SmartSchool caching infrastructure.</summary>
public static class CacheDependencyInjection
{
    /// <summary>Adds HybridCache and the configured distributed cache provider.</summary>
    public static IServiceCollection AddSmartSchoolCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(CacheOptions.SectionName)
            .Get<CacheOptions>() ?? new CacheOptions();

        services.Configure<CacheOptions>(
            configuration.GetSection(CacheOptions.SectionName));

        switch (options.Provider)
        {
            case DistributedCacheProvider.Redis:
                services.AddStackExchangeRedisCache(redis =>
                {
                    redis.InstanceName = options.InstanceName;
                    redis.Configuration =
                        configuration.GetConnectionString(options.RedisConnectionStringName)
                        ?? throw new InvalidOperationException(
                            $"Connection string '{options.RedisConnectionStringName}' is required.");
                });
                break;

            case DistributedCacheProvider.Memory:
                services.AddDistributedMemoryCache();
                break;

            case DistributedCacheProvider.PostgreSql:
            default:
                services.AddDistributedPostgresCache(postgres =>
                {
                    postgres.ConnectionString =
                        configuration.GetConnectionString("SmartSchool")
                        ?? throw new InvalidOperationException(
                            "SmartSchool PostgreSQL connection string is required.");

                    postgres.SchemaName = options.PostgreSqlSchema;
                    postgres.TableName = options.PostgreSqlTable;
                    postgres.CreateIfNotExists = true;
                });
                break;
        }

        services.AddHybridCache(hybrid =>
        {
            hybrid.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(
                    options.DefaultExpirationMinutes),
                LocalCacheExpiration = TimeSpan.FromMinutes(
                    Math.Min(options.DefaultExpirationMinutes, 5))
            };
        });

        return services;
    }
}
