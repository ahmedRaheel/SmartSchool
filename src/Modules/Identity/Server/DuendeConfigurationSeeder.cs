using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.EntityFrameworkCore;

namespace SmartSchool.Modules.Identity.Server;

/// <summary>
/// Seeds database-backed Duende configuration. Safe to run repeatedly.
/// SQL scripts contain equivalent seed data for database-first deployments.
/// </summary>
public sealed class DuendeConfigurationSeeder(
	ConfigurationDbContext dbContext,
	IConfiguration configuration)
{
	public async Task SeedAsync(CancellationToken cancellationToken = default)
	{
		if (await dbContext.Clients.AnyAsync(cancellationToken))
		{
			return;
		}

		var portalUrl = configuration["DuendeIdentityServer:PortalUrl"] ?? "https://localhost:5173";
		var mobileRedirect = configuration["DuendeIdentityServer:MobileRedirectUri"] ?? "smartschool://oauth/callback";

		var identityResources = new IdentityResource[]
		{
			new IdentityResources.OpenId(),
			new IdentityResources.Profile(),
			new IdentityResources.Email(),
			new("smartschool.profile", ["tenant_id", "given_name", "family_name", "name", "role"])
		};

		var apiScopes = new[]
		{
			new ApiScope("smartschool.api", "SmartSchool API",
				["tenant_id", "role", "given_name", "family_name", "name"])
		};

		var apiResources = new[]
		{
			new ApiResource("smartschool-api", "SmartSchool API")
			{
				Scopes = { "smartschool.api" },
				UserClaims = { "tenant_id", "role", "given_name", "family_name", "name", "email" }
			}
		};

		var clients = new[]
		{
			new Client
			{
				ClientId = "smartschool-portal",
				ClientName = "SmartSchool Portal",
				AllowedGrantTypes = GrantTypes.Code,
				RequirePkce = true,
				RequireClientSecret = false,
				AllowOfflineAccess = true,
				AllowedScopes = { "openid", "profile", "email", "smartschool.profile", "smartschool.api" },
				RedirectUris = { $"{portalUrl}/auth/callback" },
				PostLogoutRedirectUris = { portalUrl },
				AllowedCorsOrigins = { portalUrl }
			},
			new Client
			{
				ClientId = "smartschool-mobile",
				ClientName = "SmartSchool Mobile",
				AllowedGrantTypes = GrantTypes.Code,
				RequirePkce = true,
				RequireClientSecret = false,
				AllowOfflineAccess = true,
				AllowedScopes = { "openid", "profile", "email", "smartschool.profile", "smartschool.api" },
				RedirectUris = { mobileRedirect },
				PostLogoutRedirectUris = { mobileRedirect }
			}
		};

		dbContext.IdentityResources.AddRange(identityResources.Select(x => x.ToEntity()));
		dbContext.ApiScopes.AddRange(apiScopes.Select(x => x.ToEntity()));
		dbContext.ApiResources.AddRange(apiResources.Select(x => x.ToEntity()));
		dbContext.Clients.AddRange(clients.Select(x => x.ToEntity()));
		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
