using SmartSchool.Application.Persistence;
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
			.AddOptions<SmartSchool.Infrastructure.Options.AuthenticationOptions>()
			.Bind(configuration.GetSection(SmartSchool.Infrastructure.Options.AuthenticationOptions.SectionName))
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
		var persistenceOptions = configuration
			.GetSection(PersistenceOptions.SectionName)
			.Get<PersistenceOptions>() ?? new PersistenceOptions();

		var connectionString = configuration.GetConnectionString(
			persistenceOptions.ConnectionStringName);

		services.AddDbContext<ApplicationDbContext>(dbContextOptions =>
		{
			switch (persistenceOptions.Provider)
			{
				case PersistenceProvider.Mock:
					dbContextOptions.UseInMemoryDatabase("SmartSchoolDevelopment");
					break;

				case PersistenceProvider.PostgreSql:
					EnsureConnectionString(
						connectionString,
						persistenceOptions.ConnectionStringName);

					dbContextOptions.UseNpgsql(
						connectionString,
						providerOptions =>
						{
							providerOptions.EnableRetryOnFailure(5);
						});
					break;

				case PersistenceProvider.SqlServer:
					EnsureConnectionString(
						connectionString,
						persistenceOptions.ConnectionStringName);

					dbContextOptions.UseSqlServer(
						connectionString,
						providerOptions =>
						{
							providerOptions.EnableRetryOnFailure(5);
						});
					break;

				default:
					throw new InvalidOperationException(
						$"Unsupported persistence provider '{persistenceOptions.Provider}'.");
			}

			if (persistenceOptions.EnableSensitiveDataLogging)
			{
				dbContextOptions.EnableSensitiveDataLogging();
			}
		});

		services.AddScoped<IApplicationDbContext>(
			serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
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
			.GetSection(SmartSchool.Infrastructure.Options.AuthenticationOptions.SectionName)
			.Get<SmartSchool.Infrastructure.Options.AuthenticationOptions>() ?? new SmartSchool.Infrastructure.Options.AuthenticationOptions();

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
