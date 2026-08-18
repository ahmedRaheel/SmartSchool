using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartSchool.Infrastructure.Caching;
using SmartSchool.Infrastructure.Identity;
using SmartSchool.Infrastructure.Persistence;

namespace SmartSchool.Infrastructure.DependencyInjection;

/// <summary>
/// Registers configurable persistence, caching and authentication infrastructure.
/// </summary>
public static class DataPlatformServiceCollectionExtensions
{
	public static IServiceCollection AddSmartSchoolDataPlatform(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services
			.AddOptions<PersistenceOptions>()
			.Bind(configuration.GetSection(PersistenceOptions.SectionName))
			.ValidateOnStart();

		services
			.AddOptions<CacheOptions>()
			.Bind(configuration.GetSection(CacheOptions.SectionName))
			.ValidateOnStart();

		services
			.AddOptions<IdentityOptions>()
			.Bind(configuration.GetSection(IdentityOptions.SectionName))
			.ValidateOnStart();

		AddPersistence(services, configuration);
		AddCaching(services, configuration);
		AddAuthentication(services, configuration);

		return services;
	}

	private static void AddPersistence(
		IServiceCollection services,
		IConfiguration configuration)
	{
		var options = configuration
			.GetSection(PersistenceOptions.SectionName)
			.Get<PersistenceOptions>() ?? new PersistenceOptions();

		var connectionString = configuration.GetConnectionString(
			options.ConnectionStringName);

		switch (options.Provider)
		{
			case PersistenceProvider.Mock:
				services.AddDbContext<SmartSchoolMockDbContext>(
					dbOptions => dbOptions.UseInMemoryDatabase("SmartSchool"));
				break;

			case PersistenceProvider.PostgreSql:
				EnsureConnectionString(connectionString, options.ConnectionStringName);
				services.AddDbContext<SmartSchoolDbContext>(
					dbOptions => dbOptions.UseNpgsql(connectionString));
				break;

			case PersistenceProvider.SqlServer:
				EnsureConnectionString(connectionString, options.ConnectionStringName);
				services.AddDbContext<SmartSchoolDbContext>(
					dbOptions => dbOptions.UseSqlServer(connectionString));
				break;

			default:
				throw new InvalidOperationException(
					$"Unsupported persistence provider '{options.Provider}'.");
		}
	}

	private static void AddCaching(
		IServiceCollection services,
		IConfiguration configuration)
	{
		var options = configuration
			.GetSection(CacheOptions.SectionName)
			.Get<CacheOptions>() ?? new CacheOptions();

		if (options.Provider == CacheProvider.Redis)
		{
			var redisConnectionString = configuration.GetConnectionString(
				options.RedisConnectionStringName);

			EnsureConnectionString(
				redisConnectionString,
				options.RedisConnectionStringName);

			services.AddStackExchangeRedisCache(redisOptions =>
			{
				redisOptions.Configuration = redisConnectionString;
				redisOptions.InstanceName = options.InstanceName;
			});
		}
		else
		{
			services.AddDistributedMemoryCache();
		}

		services.AddHybridCache(hybridOptions =>
		{
			hybridOptions.DefaultEntryOptions = new HybridCacheEntryOptions
			{
				Expiration = TimeSpan.FromMinutes(options.DefaultExpirationMinutes),
				LocalCacheExpiration = TimeSpan.FromMinutes(
					Math.Min(options.DefaultExpirationMinutes, 5))
			};
		});
	}

	private static void AddAuthentication(
		IServiceCollection services,
		IConfiguration configuration)
	{
		var options = configuration
			.GetSection(IdentityOptions.SectionName)
			.Get<IdentityOptions>() ?? new IdentityOptions();

		if (options.Provider == IdentityProvider.Mock)
		{
			services
				.AddAuthentication(MockAuthenticationHandler.SchemeName)
				.AddScheme<
					Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions,
					MockAuthenticationHandler>(
					MockAuthenticationHandler.SchemeName,
					_ => { });

			services.AddAuthorization();
			return;
		}

		services
			.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(jwtOptions =>
			{
				jwtOptions.Authority = options.Authority;
				jwtOptions.Audience = options.Audience;
				jwtOptions.RequireHttpsMetadata = options.RequireHttpsMetadata;
				jwtOptions.MapInboundClaims = false;
			});

		services.AddAuthorization();
	}

	private static void EnsureConnectionString(
		string? connectionString,
		string connectionStringName)
	{
		if (string.IsNullOrWhiteSpace(connectionString))
		{
			throw new InvalidOperationException(
				$"Connection string '{connectionStringName}' is required.");
		}
	}
}
