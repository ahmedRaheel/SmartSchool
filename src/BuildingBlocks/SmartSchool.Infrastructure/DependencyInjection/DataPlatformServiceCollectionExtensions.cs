using SmartSchool.Application.Identity;
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
			.AddOptions<Options.AuthenticationOptions>()
			.Bind(configuration.GetSection(Options.AuthenticationOptions.SectionName))
			.ValidateOnStart();

		AddPersistence(services, configuration);
		AddCaching(services, configuration);
		AddAuthentication(services, configuration);
        services
            .AddOptions<IdentityServiceOptions>()
            .Bind(configuration.GetSection(IdentityServiceOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.BaseUrl),
                "IdentityService:BaseUrl is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ClientId),
                "IdentityService:ClientId is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.ClientSecret),
                "IdentityService:ClientSecret is required.")
            .Validate(options => !string.IsNullOrWhiteSpace(options.Scope),
                "IdentityService:Scope is required.")
            .ValidateOnStart();

        var identityServiceOptions = configuration
            .GetRequiredSection(IdentityServiceOptions.SectionName)
            .Get<IdentityServiceOptions>()
            ?? throw new InvalidOperationException(
                "IdentityService configuration is required.");

        services.AddHttpClient<IIdentityAccountService, IdentityAccountService>(client =>
        {
            client.BaseAddress = new Uri(
                identityServiceOptions.BaseUrl.TrimEnd('/') + "/",
                UriKind.Absolute);
            client.Timeout = TimeSpan.FromSeconds(30);
        });


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

		services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
		services.AddScoped<IBusinessNumberGenerator, BusinessNumberGenerator>();
		services.AddScoped<IApplicationDbContext>(
			serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());
		services.AddScoped<MockDatabaseSeeder>();
		services.AddScoped<IEfMockStore, EfMockStore>();
	}

	private static void AddCaching(
		IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddSmartSchoolCaching(configuration);
	}

	private static void AddAuthentication(
		IServiceCollection services,
		IConfiguration configuration)
	{
		var options = configuration
			.GetSection(Options.AuthenticationOptions.SectionName)
			.Get<Options.AuthenticationOptions>() ?? new Options.AuthenticationOptions();

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
				jwtOptions.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];
						var path = context.HttpContext.Request.Path;

						if (!string.IsNullOrEmpty(accessToken)
							&& (path.StartsWithSegments("/hubs/notifications")
								|| path.StartsWithSegments("/hubs/chat")))
						{
							context.Token = accessToken;
						}

						return Task.CompletedTask;
					}
				};
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
