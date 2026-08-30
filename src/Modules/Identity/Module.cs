using SmartSchool.Modules.Identity.Features.ServiceAccounts;
using SmartSchool.Modules.Identity.Features.Account;
using SmartSchool.Modules.Identity.Features.Roles;
using SmartSchool.Modules.Identity.Features.Users;
using SmartSchool.Modules.Identity.Server;
using SmartSchool.Modules.Identity.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using SmartSchool.Application;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Identity;
using SmartSchool.Modules.Identity.Infrastructure;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.Identity.Features.RoleAssignment;
using SmartSchool.Modules.Identity.Features.UserProfile;
namespace SmartSchool.Modules.Identity;

public static class Module
{
	public static IServiceCollection AddIdentityModule(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddFeaturePersistence(typeof(Module).Assembly);

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

				var issuerUri = configuration["DuendeIdentityServer:IssuerUri"];
				if (!string.IsNullOrWhiteSpace(issuerUri))
				{
					options.IssuerUri = issuerUri.TrimEnd('/');
				}

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

		services.AddOptions<Features.ServiceAccounts.AccountProvisioningEndpoints.AccountProvisioningOptions>()
			.Bind(configuration.GetSection(Features.ServiceAccounts.AccountProvisioningEndpoints.AccountProvisioningOptions.SectionName))
			.Validate(options => !string.IsNullOrWhiteSpace(options.TemporaryPassword), "AccountProvisioning:TemporaryPassword is required.")
			.ValidateOnStart();

		services.AddHttpContextAccessor();
		services.AddScoped<SmartSchool.Application.Identity.ICurrentUser, SmartSchool.Application.Identity.CurrentUser>();
		services.AddScoped<SmartSchool.Application.Identity.ITenantScope, SmartSchool.Application.Identity.TenantScope>();

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

		CreateRoleAssignment.MapEndpoint(endpoints);
		CreateUserProfile.MapEndpoint(endpoints);
		DeleteRoleAssignment.MapEndpoint(endpoints);
		DeleteUserProfile.MapEndpoint(endpoints);
		GetRoleAssignmentById.MapEndpoint(endpoints);
		GetRoleAssignmentPage.MapEndpoint(endpoints);
		GetUserProfileById.MapEndpoint(endpoints);
		GetUserProfilePage.MapEndpoint(endpoints);
		UpdateRoleAssignment.MapEndpoint(endpoints);
		UpdateUserProfile.MapEndpoint(endpoints);

		return endpoints;
	}
}
