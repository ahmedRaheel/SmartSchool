using SmartSchool.Modules.Identity.Features.ServiceAccounts;
using SmartSchool.Modules.Identity.Features.Account;
using SmartSchool.Modules.Identity.Features.Roles;
using SmartSchool.Modules.Identity.Features.Users;
using SmartSchool.Modules.Identity.Server;
using SmartSchool.Modules.Identity.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Modules.Identity.Persistence;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity;

public static class Module
{
	public static IServiceCollection AddIdentityModule(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		var provider = configuration["Persistence:Provider"] ?? IdentityDatabaseProvider.PostgreSql;
		var connectionStringName = configuration["Persistence:ConnectionStringName"] ?? "SmartSchool";
		var connectionString = configuration.GetConnectionString(connectionStringName)
			?? throw new InvalidOperationException(
				$"Connection string '{connectionStringName}' is required for Identity.");

		var migrationsAssembly = typeof(Module).Assembly.GetName().Name
			?? throw new InvalidOperationException("Unable to resolve Identity migrations assembly.");

		services.AddDbContext<SmartSchoolIdentityDbContext>(options =>
			IdentityDatabaseProvider.Configure(
				options,
				provider,
				connectionString,
				migrationsAssembly,
				"__EFMigrationsHistory_AspNetIdentity",
				"identity"));

		services
			.AddIdentity<SmartSchoolUser, SmartSchoolRole>(options =>
			{
				options.Password.RequiredLength = 8;
				options.Password.RequireDigit = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = false;
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.User.RequireUniqueEmail = true;
			})
			.AddEntityFrameworkStores<SmartSchoolIdentityDbContext>()
			.AddDefaultTokenProviders();

		var identityServer = services
			.AddIdentityServer(options =>
			{
				options.Events.RaiseErrorEvents = true;
				options.Events.RaiseInformationEvents = true;
				options.Events.RaiseFailureEvents = true;
				options.Events.RaiseSuccessEvents = true;

				var licenseKey = configuration["DuendeIdentityServer:LicenseKey"];
				if (!string.IsNullOrWhiteSpace(licenseKey))
				{
					options.LicenseKey = licenseKey;
				}
			})
			.AddAspNetIdentity<SmartSchoolUser>()
			.AddConfigurationStore(options =>
			{
				options.DefaultSchema = "identity";
				options.ConfigureDbContext = db =>
					IdentityDatabaseProvider.Configure(
						db,
						provider,
						connectionString,
						migrationsAssembly,
						"__EFMigrationsHistory_DuendeConfiguration",
						"identity");
			})
			.AddOperationalStore(options =>
			{
				options.DefaultSchema = "identity";
				options.EnableTokenCleanup = true;
				options.TokenCleanupInterval = 3600;
				options.ConfigureDbContext = db =>
					IdentityDatabaseProvider.Configure(
						db,
						provider,
						connectionString,
						migrationsAssembly,
						"__EFMigrationsHistory_DuendeOperational",
						"identity");
			})
			.AddConfigurationStoreCache()
			.AddProfileService<SmartSchoolProfileService>();

		if (configuration.GetValue<bool>("DuendeIdentityServer:UseDeveloperSigningCredential"))
		{
			identityServer.AddDeveloperSigningCredential();
		}

		services.AddHttpClient("IdentityTokenClient");
		services.AddTransient<Duende.IdentityServer.Validation.IExtensionGrantValidator, ImpersonationGrantValidator>();

		services.AddScoped<IdentityDataSeeder>();
		services.AddScoped<DuendeConfigurationSeeder>();
		return services;
	}

	public static IEndpointRouteBuilder MapIdentityServerEndpoints(
		this IEndpointRouteBuilder endpoints)
	{
		UserManagementEndpoints.MapEndpoints(endpoints);
		RoleManagementEndpoints.MapEndpoints(endpoints);
		AccountEndpoints.MapEndpoints(endpoints);
		AccountProvisioningEndpoints.MapEndpoints(endpoints);

		return endpoints;
	}

	/// <summary>
	/// Maps legacy SmartSchool business-profile CQRS endpoints.
	/// These belong to SmartSchool.Api, not the dedicated Identity host.
	/// </summary>
	public static IEndpointRouteBuilder MapIdentityBusinessEndpoints(
		this IEndpointRouteBuilder endpoints)
	{

		return endpoints;
	}
}
