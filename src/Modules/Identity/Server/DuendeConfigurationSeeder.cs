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

        var portalUrl = configuration["DuendeIdentityServer:PortalUrl"] ?? throw new InvalidOperationException("DuendeIdentityServer:PortalUrl configuration is required.");
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
                ["tenant_id", "role", "given_name", "family_name", "name"]),
            new ApiScope("smartschool.identity.manage", "SmartSchool account lifecycle management")
        };

        var apiResources = new[]
        {
            new ApiResource("smartschool-api", "SmartSchool API")
            {
                Scopes = { "smartschool.api" },
                UserClaims =
                {
                    "tenant_id", "school_id", "branch_id", "student_id", "teacher_id",
                    "driver_id", "examiner_id", "employee_id", "role", "given_name",
                    "family_name", "name", "email", "account_type", "must_change_password"
                }
            }
        };

        var serviceClientSecret =
            configuration["SmartSchoolApiClient:ClientSecret"]
            ?? throw new InvalidOperationException(
                "SmartSchoolApiClient:ClientSecret is required.");

        var clients = new[]
        {
            new Client
            {
                ClientId = "smartschool-api-service",
                ClientName = "SmartSchool API Service",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret(serviceClientSecret.Sha256()) },
                AllowedScopes = { "smartschool.identity.manage" }
            },
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

        var loginClientSecret = configuration["LoginApiClient:ClientSecret"]
            ?? throw new InvalidOperationException("LoginApiClient:ClientSecret is required.");

        var allClients = clients.Append(new Client
        {
            ClientId = configuration["LoginApiClient:ClientId"] ?? throw new InvalidOperationException("LoginApiClient:ClientId is required."),
            ClientName = "SmartSchool Login API",
            AllowedGrantTypes = [.. GrantTypes.ResourceOwnerPassword, ImpersonationGrantValidator.GrantTypeName],
            ClientSecrets = { new Secret(loginClientSecret.Sha256()) },
            AllowOfflineAccess = true,
            AllowedScopes = { "openid", "profile", "email", "smartschool.profile", "smartschool.api", "offline_access" },
            AccessTokenLifetime = 3600
        });

        foreach (var resource in identityResources)
            if (!await dbContext.IdentityResources.AnyAsync(x => x.Name == resource.Name, cancellationToken))
                dbContext.IdentityResources.Add(resource.ToEntity());
        foreach (var scope in apiScopes)
            if (!await dbContext.ApiScopes.AnyAsync(x => x.Name == scope.Name, cancellationToken))
                dbContext.ApiScopes.Add(scope.ToEntity());
        foreach (var resource in apiResources)
            if (!await dbContext.ApiResources.AnyAsync(x => x.Name == resource.Name, cancellationToken))
                dbContext.ApiResources.Add(resource.ToEntity());
        foreach (var client in allClients)
            if (!await dbContext.Clients.AnyAsync(x => x.ClientId == client.ClientId, cancellationToken))
                dbContext.Clients.Add(client.ToEntity());

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
