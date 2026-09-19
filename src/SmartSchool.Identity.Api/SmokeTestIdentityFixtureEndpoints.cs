using System.Security.Cryptography;
using System.Text;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Identity.Api;

/// <summary>
/// Development-only bootstrap endpoints for the self-contained API smoke suite.
/// These endpoints deliberately do not use the application's bearer pipeline,
/// because the smoke suite must be able to provision test identities before it
/// can validate bearer authentication itself.
/// </summary>
internal static class SmokeTestIdentityFixtureEndpoints
{
    private const string HeaderName = "X-Smoke-Test-Key";
    private const string RunHeaderName = "X-Smoke-Test-Run";
    private const string TestEmailSuffix = "@smartschool.test";
    private const string ProtocolVersion = "2026.09.19-smoke-bootstrap.2";

    public sealed record CreateUserRequest(
        string RunId,
        Guid? TenantId,
        Guid? SchoolId,
        Guid? BranchId,
        Guid? BusinessEntityId,
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string AccountType,
        string[] Roles);

    public static IEndpointRouteBuilder MapSmokeTestIdentityFixtureEndpoints(
        this IEndpointRouteBuilder endpoints,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        if (!environment.IsDevelopment())
        {
            return endpoints;
        }

        var configuredKey = configuration["SmokeTesting:BootstrapKey"];
        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            return endpoints;
        }

        var group = endpoints
            .MapGroup("/api/testing/smoke/identity")
            .WithTags("Testing - Identity Fixture");

        group.MapPost("/users", CreateUserAsync);
        group.MapDelete("/users/{userId:guid}", DeleteUserAsync);
        group.MapDelete("/tenants/{tenantId:guid}/users", DeleteTenantUsersAsync);
        group.MapGet("/probe", Probe);

        return endpoints;
    }


    private static IResult Probe(HttpContext context, IConfiguration configuration)
    {
        if (!Authorized(context, configuration))
        {
            return Results.Unauthorized();
        }

        if (!TryGetAuthorizedRunId(context, suppliedRunId: null, out var runId))
        {
            return Results.BadRequest(new
            {
                message = "X-Smoke-Test-Run is missing or invalid.",
                protocolVersion = ProtocolVersion
            });
        }

        return Results.Ok(new
        {
            status = "ready",
            protocolVersion = ProtocolVersion,
            runId
        });
    }

    private static async Task<IResult> CreateUserAsync(
        CreateUserRequest request,
        HttpContext httpContext,
        IConfiguration configuration,
        UserManager<SmartSchoolUser> userManager,
        CancellationToken cancellationToken)
    {
        if (!Authorized(httpContext, configuration))
        {
            return Results.Unauthorized();
        }

        if (!TryGetAuthorizedRunId(httpContext, suppliedRunId: null, out var authorizedRunId))
        {
            return Results.BadRequest(new
            {
                message = "X-Smoke-Test-Run is missing or invalid."
            });
        }

        var normalizedEmail = request.Email?.Trim();
        if (!IsValidRunOwnedEmail(authorizedRunId, normalizedEmail))
        {
            return Results.BadRequest(new
            {
                message = "Smoke-test users must use the run-owned @smartschool.test email namespace.",
                expectedPrefix = authorizedRunId + ".",
                expectedSuffix = TestEmailSuffix
            });
        }

        var existing = await userManager.FindByEmailAsync(normalizedEmail!);
        if (existing is not null)
        {
            return Results.Conflict(new { message = "Smoke-test user already exists.", userId = existing.Id });
        }

        var user = new SmartSchoolUser
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            SchoolId = request.SchoolId,
            BranchId = request.BranchId,
            BusinessEntityId = request.BusinessEntityId,
            AccountType = request.AccountType,
            UserName = normalizedEmail,
            Email = normalizedEmail,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DisplayName = $"{request.FirstName} {request.LastName}".Trim(),
            EmailConfirmed = true,
            IsActive = true,
            MustChangePassword = false
        };

        var create = await userManager.CreateAsync(user, request.Password);
        if (!create.Succeeded)
        {
            return Results.ValidationProblem(ToErrors(create));
        }

        var roles = request.Roles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (roles.Length > 0)
        {
            var roleResult = await userManager.AddToRolesAsync(user, roles);
            if (!roleResult.Succeeded)
            {
                await userManager.DeleteAsync(user);
                return Results.ValidationProblem(ToErrors(roleResult));
            }
        }

        return Results.Created(
            $"/api/testing/smoke/identity/users/{user.Id}",
            new
            {
                user = new
                {
                    id = user.Id,
                    user.TenantId,
                    user.SchoolId,
                    user.BranchId,
                    user.BusinessEntityId,
                    user.Email,
                    user.AccountType,
                    roles
                }
            });
    }

    private static async Task<IResult> DeleteUserAsync(
        Guid userId,
        string runId,
        HttpContext httpContext,
        IConfiguration configuration,
        UserManager<SmartSchoolUser> userManager,
        PersistedGrantDbContext operationalDbContext,
        CancellationToken cancellationToken)
    {
        if (!Authorized(httpContext, configuration))
        {
            return Results.Unauthorized();
        }

        if (!TryGetAuthorizedRunId(httpContext, suppliedRunId: null, out var authorizedRunId))
        {
            return Results.BadRequest(new { message = "X-Smoke-Test-Run is missing or invalid." });
        }

        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            return Results.NotFound();
        }

        if (!IsValidRunOwnedEmail(authorizedRunId, user.Email))
        {
            return Results.Forbid();
        }

        await DeleteOperationalArtifactsAsync(user.Id, operationalDbContext, cancellationToken);
        var result = await userManager.DeleteAsync(user);
        return result.Succeeded
            ? Results.NoContent()
            : Results.ValidationProblem(ToErrors(result));
    }

    private static async Task<IResult> DeleteTenantUsersAsync(
        Guid tenantId,
        string runId,
        HttpContext httpContext,
        IConfiguration configuration,
        UserManager<SmartSchoolUser> userManager,
        PersistedGrantDbContext operationalDbContext,
        CancellationToken cancellationToken)
    {
        if (!Authorized(httpContext, configuration))
        {
            return Results.Unauthorized();
        }

        if (!TryGetAuthorizedRunId(httpContext, suppliedRunId: null, out var authorizedRunId))
        {
            return Results.BadRequest(new { message = "X-Smoke-Test-Run is missing or invalid." });
        }

        var prefix = authorizedRunId + ".";
        var users = await userManager.Users
            .Where(user =>
                user.TenantId == tenantId
                && user.Email != null
                && user.Email.StartsWith(prefix)
                && user.Email.EndsWith(TestEmailSuffix))
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            await DeleteOperationalArtifactsAsync(user.Id, operationalDbContext, cancellationToken);
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Results.ValidationProblem(ToErrors(result));
            }
        }

        return Results.Ok(new { deleted = users.Count });
    }

    private static bool Authorized(HttpContext context, IConfiguration configuration)
    {
        var expected = configuration["SmokeTesting:BootstrapKey"];
        if (string.IsNullOrWhiteSpace(expected)
            || !context.Request.Headers.TryGetValue(HeaderName, out var provided)
            || string.IsNullOrWhiteSpace(provided))
        {
            return false;
        }

        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        var providedBytes = Encoding.UTF8.GetBytes(provided.ToString());
        return expectedBytes.Length == providedBytes.Length
            && CryptographicOperations.FixedTimeEquals(expectedBytes, providedBytes);
    }


    private static bool TryGetAuthorizedRunId(
        HttpContext context,
        string? suppliedRunId,
        out string runId)
    {
        runId = string.Empty;

        if (!context.Request.Headers.TryGetValue(RunHeaderName, out var runHeader))
        {
            return false;
        }

        var headerRunId = runHeader.ToString().Trim();
        if (!IsValidRunId(headerRunId))
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(suppliedRunId)
            && !string.Equals(
                headerRunId,
                suppliedRunId.Trim(),
                StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        runId = headerRunId;
        return true;
    }

    private static bool IsValidRunId(string? runId)
    {
        if (string.IsNullOrWhiteSpace(runId))
        {
            return false;
        }

        var value = runId.Trim();
        return value.StartsWith("smoke-", StringComparison.OrdinalIgnoreCase)
            && value.Length <= 96
            && value.All(character =>
                char.IsLetterOrDigit(character)
                || character is '-' or '_');
    }

    private static bool IsValidRunOwnedEmail(string runId, string? email)
    {
        if (string.IsNullOrWhiteSpace(runId)
            || string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        var normalizedRunId = runId.Trim();
        var normalizedEmail = email.Trim();

        return IsValidRunId(normalizedRunId)
            && normalizedEmail.StartsWith(
                normalizedRunId + ".",
                StringComparison.OrdinalIgnoreCase)
            && normalizedEmail.EndsWith(
                TestEmailSuffix,
                StringComparison.OrdinalIgnoreCase);
    }

    private static async Task DeleteOperationalArtifactsAsync(
        Guid userId,
        PersistedGrantDbContext operationalDbContext,
        CancellationToken cancellationToken)
    {
        var subjectId = userId.ToString();

        await operationalDbContext.Database.ExecuteSqlInterpolatedAsync(
            $@"DELETE FROM identity.""PersistedGrants"" WHERE ""SubjectId"" = {subjectId};",
            cancellationToken);
        await operationalDbContext.Database.ExecuteSqlInterpolatedAsync(
            $@"DELETE FROM identity.""DeviceCodes"" WHERE ""SubjectId"" = {subjectId};",
            cancellationToken);
        await operationalDbContext.Database.ExecuteSqlInterpolatedAsync(
            $@"DELETE FROM identity.""ServerSideSessions"" WHERE ""SubjectId"" = {subjectId};",
            cancellationToken);
    }

    private static Dictionary<string, string[]> ToErrors(IdentityResult result) =>
        result.Errors
            .GroupBy(error => error.Code)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Description).ToArray());
}
