using Duende.IdentityServer.EntityFramework.DbContexts;
using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Identity.Persistence.Identity;
using SmartSchool.Modules.Identity.Server;
using Microsoft.Extensions.Hosting;

namespace SmartSchool.Modules.Identity.Features.Users;

public static class UserManagementEndpoints
{
    public sealed record CreateUserRequest(
        Guid? TenantId,
        Guid? SchoolId,
        Guid? BranchId,
        Guid? BusinessEntityId,
        string Email,
        string? Password,
        string FirstName,
        string LastName,
        string AccountType,
        string[] Roles);

    public sealed record UpdateUserRequest(
        string FirstName, string LastName, string? DisplayName,
        string? PhoneNumber, bool IsActive);

    public sealed record ResetPasswordRequest(string NewPassword);
    public sealed record SetRolesRequest(string[] Roles);
    public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);
    public sealed record TenantStatusRequest(bool IsActive);
    public sealed record ImpersonateRequest(Guid TargetUserId, string? Reason);

    public sealed record UserResponse(
        Guid Id, Guid? TenantId, Guid? SchoolId, string Email,
        string FirstName, string LastName, string? DisplayName,
        string? PhoneNumber, string? AccountType, bool IsActive,
        bool MustChangePassword, IReadOnlyList<string> Roles);

    private static readonly HashSet<string> SchoolRoles =
        Enum.GetNames<Role>().Where(role => role != nameof(Role.SuperAdmin))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/identity/users")
            .WithTags("Identity - Users")
            .RequireAuthorization();

        group.MapGet("", GetPageAsync);
        group.MapGet("/{id:guid}", GetByIdAsync);
        group.MapPost("", CreateAsync);
        group.MapPut("/{id:guid}", UpdateAsync);
        group.MapPut("/{id:guid}/roles", SetRolesAsync);
        group.MapPost("/{id:guid}/reset-password", ResetPasswordAsync);
        group.MapPost("/change-password", ChangePasswordAsync);
        group.MapPost("/{id:guid}/lock", LockAsync);
        group.MapPost("/{id:guid}/unlock", UnlockAsync);
        group.MapDelete("/{id:guid}", DeactivateAsync);

        // Platform-only hard delete used by controlled lifecycle tooling such as
        // the disposable API smoke-test fixture. Normal UI deletion remains a
        // soft-deactivation operation.
        group.MapDelete("/{id:guid}/purge", PurgeAsync)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);

        // Platform-only operations.
        group.MapPost("/tenant/{tenantId:guid}/status", SetTenantStatusAsync)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);
        group.MapDelete("/tenant/{tenantId:guid}", DeleteTenantUsersAsync)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);
        group.MapPost("/impersonation/start", StartImpersonationAsync)
            .RequireAuthorization(SmartSchoolPolicies.Impersonation);
    }

    private static async Task<IResult> GetPageAsync(
        int page, int pageSize, Guid? tenantId, [FromServices] ICurrentUser currentUser,
        [FromServices] UserManager<SmartSchoolUser> userManager, CancellationToken cancellationToken)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize <= 0 ? 20 : pageSize, 1, 100);

        var effectiveTenant = currentUser.IsSuperAdmin ? tenantId : currentUser.TenantId;
        if (!currentUser.IsSuperAdmin && effectiveTenant is null) return Results.Forbid();

        var query = userManager.Users.AsNoTracking();
        if (effectiveTenant.HasValue) query = query.Where(x => x.TenantId == effectiveTenant.Value);

        var total = await query.CountAsync(cancellationToken);
        var users = await query.OrderBy(x => x.Email)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        var result = new List<UserResponse>(users.Count);
        foreach (var user in users) result.Add(await ToResponseAsync(user, userManager));
        return Results.Ok(new { page, pageSize, total, items = result });
    }

    private static async Task<IResult> GetByIdAsync(
        Guid id, [FromServices] ICurrentUser currentUser, [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return Results.NotFound();
        if (!currentUser.IsSuperAdmin && user.TenantId != currentUser.TenantId) return Results.Forbid();
        return Results.Ok(await ToResponseAsync(user, userManager));
    }

    private static async Task<IResult> CreateAsync(
        CreateUserRequest request, [FromServices] ICurrentUser currentUser,
        [FromServices] UserManager<SmartSchoolUser> userManager,
        [FromServices] ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("SmartSchool.Identity.UserManagement");
        var superAdmin = currentUser.IsSuperAdmin;
        var callerTenant = currentUser.TenantId;

        var requestedRoles = request.Roles
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var createsSuperAdmin = requestedRoles.Contains(
            nameof(Role.SuperAdmin),
            StringComparer.OrdinalIgnoreCase);

        if (!superAdmin)
        {
            if (!request.TenantId.HasValue || callerTenant != request.TenantId)
            {
                return Results.Forbid();
            }

            if (createsSuperAdmin
                || requestedRoles.Any(role => !SchoolRoles.Contains(role)))
            {
                return Results.Forbid();
            }
        }

        if (createsSuperAdmin && request.TenantId.HasValue)
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["TenantId"] =
                    [
                        "A SuperAdmin account must not be tenant-scoped."
                    ]
                });
        }

        if (!createsSuperAdmin && !request.TenantId.HasValue)
        {
            return Results.ValidationProblem(
                new Dictionary<string, string[]>
                {
                    ["TenantId"] =
                    [
                        "TenantId is required for non-platform accounts."
                    ]
                });
        }

        var password = string.IsNullOrWhiteSpace(request.Password)
            ? TemporaryPasswordGenerator.Create()
            : request.Password;

        var user = new SmartSchoolUser
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            SchoolId = request.SchoolId,
            BranchId = request.BranchId,
            BusinessEntityId = request.BusinessEntityId,
            AccountType = request.AccountType,
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DisplayName = $"{request.FirstName} {request.LastName}".Trim(),
            EmailConfirmed = true,
            IsActive = true,
            MustChangePassword = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            logger.LogWarning(
                "Identity user creation failed for {Email} tenant {TenantId}. Errors: {Errors}",
                request.Email, request.TenantId,
                string.Join(", ", result.Errors.Select(error => $"{error.Code}: {error.Description}")));
            return Results.ValidationProblem(ToErrors(result));
        }

        if (requestedRoles.Length > 0)
        {
            var roleResult = await userManager.AddToRolesAsync(user, requestedRoles);
            if (!roleResult.Succeeded)
            {
                logger.LogWarning(
                    "Identity user {UserId} was created but role assignment failed. Roles: {Roles}. Errors: {Errors}",
                    user.Id, requestedRoles,
                    string.Join(", ", roleResult.Errors.Select(error => $"{error.Code}: {error.Description}")));
                return Results.ValidationProblem(ToErrors(roleResult));
            }
        }

        logger.LogInformation(
            "Identity user {UserId} created for tenant {TenantId}, school {SchoolId}, account type {AccountType}, roles {Roles}",
            user.Id, user.TenantId, user.SchoolId, user.AccountType, requestedRoles);

        // Temporary password is returned exactly once. ASP.NET Identity stores only its hash.
        return Results.Created($"/api/identity/users/{user.Id}", new
        {
            user = await ToResponseAsync(user, userManager),
            temporaryPassword = password
        });
    }

    private static async Task<IResult> ChangePasswordAsync(
        ChangePasswordRequest request, [FromServices] ICurrentUser currentUser,
        [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user = await userManager.FindByIdAsync(currentUser.UserId.ToString());
        if (user is null) return Results.Unauthorized();

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded) return Results.ValidationProblem(ToErrors(result));

        user.MustChangePassword = false;
        user.PasswordChangedAt = DateTimeOffset.UtcNow;
        user.UpdatedAt = DateTimeOffset.UtcNow;
        await userManager.UpdateAsync(user);
        return Results.NoContent();
    }

    private static async Task<IResult> SetTenantStatusAsync(
        Guid tenantId, TenantStatusRequest request,
        [FromServices] UserManager<SmartSchoolUser> userManager, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.Where(x => x.TenantId == tenantId).ToListAsync(cancellationToken);
        foreach (var user in users)
        {
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            await userManager.UpdateAsync(user);
            if (request.IsActive)
            {
                await userManager.SetLockoutEndDateAsync(user, null);
                await userManager.ResetAccessFailedCountAsync(user);
            }
            else
            {
                await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            }
        }
        return Results.Ok(new { tenantId, isActive = request.IsActive, affectedUsers = users.Count });
    }

    private static async Task<IResult> DeleteTenantUsersAsync(
        Guid tenantId,
        [FromServices] UserManager<SmartSchoolUser> userManager,
        [FromServices] PersistedGrantDbContext operationalDbContext,
        CancellationToken cancellationToken)
    {
        var users = await userManager.Users
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            await DeleteOperationalArtifactsAsync(
                user.Id,
                operationalDbContext,
                cancellationToken);

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return Results.ValidationProblem(ToErrors(result));
            }
        }

        return Results.Ok(new { tenantId, deletedUsers = users.Count });
    }

    // Creates an audited support impersonation and performs the confidential-client
    // token exchange on the Identity host so no client secret is ever exposed to the browser.
    private static async Task<IResult> StartImpersonationAsync(
        ImpersonateRequest request,
        HttpContext httpContext,
        [FromServices] ICurrentUser currentUser,
        [FromServices] UserManager<SmartSchoolUser> userManager,
        [FromServices] IHttpClientFactory httpClientFactory,
        [FromServices] IConfiguration configuration,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var target = await userManager.FindByIdAsync(request.TargetUserId.ToString());
        if (target is null || !target.IsActive) return Results.NotFound();

        var isSuperAdmin = currentUser.IsSuperAdmin;
        var callerTenant = currentUser.TenantId;
        if (!isSuperAdmin && (!callerTenant.HasValue || target.TenantId != callerTenant.Value))
            return Results.Forbid();

        var roles = await userManager.GetRolesAsync(target);
        if (!isSuperAdmin && roles.Contains(SmartSchoolRoles.SuperAdmin, StringComparer.OrdinalIgnoreCase))
            return Results.Forbid();

        var authorization = httpContext.Request.Headers.Authorization.ToString();
        if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return Results.Unauthorized();

        var actorToken = authorization["Bearer ".Length..].Trim();
        if (string.IsNullOrWhiteSpace(actorToken)) return Results.Unauthorized();

        var clientId = configuration["LoginApiClient:ClientId"]
            ?? throw new InvalidOperationException("LoginApiClient:ClientId is required.");
        var clientSecret = configuration["LoginApiClient:ClientSecret"];
        if (string.IsNullOrWhiteSpace(clientSecret))
            throw new InvalidOperationException("LoginApiClient:ClientSecret is required.");
        var tokenEndpoint = configuration["LoginApiClient:TokenEndpoint"]
            ?? throw new InvalidOperationException("LoginApiClient:TokenEndpoint is required.");

        var reason = string.IsNullOrWhiteSpace(request.Reason) ? "Support session" : request.Reason.Trim();
        var impersonatorId = currentUser.UserId;
        loggerFactory.CreateLogger("SmartSchool.Impersonation").LogWarning(
            "Administrator {ImpersonatorId} started support impersonation for {TargetUserId} tenant {TenantId}. Reason: {Reason}",
            impersonatorId, target.Id, target.TenantId, reason);

        using var tokenRequest = new HttpRequestMessage(HttpMethod.Post, tokenEndpoint)
        {
            Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = ImpersonationGrantValidator.GrantTypeName,
                ["client_id"] = clientId,
                ["client_secret"] = clientSecret,
                ["actor_token"] = actorToken,
                ["target_user_id"] = target.Id.ToString(),
                ["reason"] = reason,
                // The impersonated token is used against SmartSchool.Api.
                // Without an explicit API scope Duende can issue a token that
                // has no smartschool-api audience, which the API correctly
                // rejects with HTTP 401.
                ["scope"] = "smartschool.api"
            })
        };

        using var tokenResponse = await httpClientFactory.CreateClient("IdentityTokenClient")
            .SendAsync(tokenRequest, cancellationToken);
        var tokenJson = await tokenResponse.Content.ReadAsStringAsync(cancellationToken);
        if (!tokenResponse.IsSuccessStatusCode)
        {
            loggerFactory.CreateLogger("SmartSchool.Impersonation").LogWarning(
                "Impersonation token exchange failed with status {StatusCode} for target {TargetUserId}.",
                (int)tokenResponse.StatusCode, target.Id);
            return Results.Json(
                new { message = "Unable to start impersonation." },
                statusCode: (int)tokenResponse.StatusCode);
        }

        using var document = JsonDocument.Parse(tokenJson);
        var root = document.RootElement;
        return Results.Ok(new
        {
            accessToken = root.GetProperty("access_token").GetString(),
            tokenType = root.TryGetProperty("token_type", out var tokenType) ? tokenType.GetString() ?? "Bearer" : "Bearer",
            expiresIn = root.TryGetProperty("expires_in", out var expiresIn) ? expiresIn.GetInt32() : 0,
            refreshToken = root.TryGetProperty("refresh_token", out var refreshToken) ? refreshToken.GetString() : null,
            targetUser = await ToResponseAsync(target, userManager)
        });
    }

    private static async Task<IResult> UpdateAsync(Guid id, UpdateUserRequest request, [FromServices] ICurrentUser currentUser, [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return Results.NotFound();
        if (!currentUser.IsSuperAdmin && user.TenantId != currentUser.TenantId) return Results.Forbid();
        user.FirstName=request.FirstName; user.LastName=request.LastName; user.DisplayName=request.DisplayName;
        user.PhoneNumber=request.PhoneNumber; user.IsActive=request.IsActive; user.UpdatedAt=DateTimeOffset.UtcNow;
        var result=await userManager.UpdateAsync(user);
        return result.Succeeded ? Results.Ok(await ToResponseAsync(user,userManager)) : Results.ValidationProblem(ToErrors(result));
    }

    private static async Task<IResult> SetRolesAsync(Guid id, SetRolesRequest request, [FromServices] ICurrentUser currentUser, [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!currentUser.IsSuperAdmin && user.TenantId != currentUser.TenantId) return Results.Forbid();
        if (!currentUser.IsSuperAdmin && request.Roles.Any(r => !SchoolRoles.Contains(r) || r.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase))) return Results.Forbid();
        var current=await userManager.GetRolesAsync(user);
        var remove=await userManager.RemoveFromRolesAsync(user,current);
        if(!remove.Succeeded) return Results.ValidationProblem(ToErrors(remove));
        var add=await userManager.AddToRolesAsync(user,request.Roles.Distinct(StringComparer.OrdinalIgnoreCase));
        return add.Succeeded ? Results.Ok(await ToResponseAsync(user,userManager)) : Results.ValidationProblem(ToErrors(add));
    }

    private static async Task<IResult> ResetPasswordAsync(Guid id, ResetPasswordRequest request, [FromServices] ICurrentUser currentUser, [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!currentUser.IsSuperAdmin && user.TenantId != currentUser.TenantId) return Results.Forbid();
        var token=await userManager.GeneratePasswordResetTokenAsync(user);
        var result=await userManager.ResetPasswordAsync(user,token,request.NewPassword);
        if (result.Succeeded)
        {
            user.MustChangePassword = true;
            await userManager.UpdateAsync(user);
            return Results.NoContent();
        }
        return Results.ValidationProblem(ToErrors(result));
    }

    private static async Task<IResult> LockAsync(Guid id, [FromServices] ICurrentUser currentUser, [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!currentUser.IsSuperAdmin && user.TenantId != currentUser.TenantId) return Results.Forbid();
        await userManager.SetLockoutEndDateAsync(user,DateTimeOffset.UtcNow.AddYears(100));
        return Results.NoContent();
    }

    private static async Task<IResult> UnlockAsync(Guid id, [FromServices] ICurrentUser currentUser, [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!currentUser.IsSuperAdmin && user.TenantId != currentUser.TenantId) return Results.Forbid();
        await userManager.SetLockoutEndDateAsync(user,null);
        await userManager.ResetAccessFailedCountAsync(user);
        return Results.NoContent();
    }

    private static async Task<IResult> DeactivateAsync(Guid id, [FromServices] ICurrentUser currentUser, [FromServices] UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!currentUser.IsSuperAdmin && user.TenantId != currentUser.TenantId) return Results.Forbid();
        user.IsActive=false; user.UpdatedAt=DateTimeOffset.UtcNow;
        await userManager.UpdateAsync(user);
        return Results.NoContent();
    }



    private static async Task<IResult> PurgeAsync(
        Guid id,
        [FromServices] ICurrentUser currentUser,
        [FromServices] UserManager<SmartSchoolUser> userManager,
        [FromServices] PersistedGrantDbContext operationalDbContext,
        [FromServices] IHostEnvironment environment,
        CancellationToken cancellationToken)
    {
        if (!environment.IsDevelopment())
        {
            return Results.NotFound();
        }

        if (!currentUser.IsSuperAdmin)
        {
            return Results.Forbid();
        }

        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
        {
            return Results.NotFound();
        }

        if (user.Id == currentUser.UserId)
        {
            return Results.BadRequest(
                new
                {
                    message =
                        "A SuperAdmin cannot purge the account represented by the current token."
                });
        }

        await DeleteOperationalArtifactsAsync(
            user.Id,
            operationalDbContext,
            cancellationToken);

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded
            ? Results.NoContent()
            : Results.ValidationProblem(ToErrors(result));
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

    private static async Task<UserResponse> ToResponseAsync(SmartSchoolUser user, UserManager<SmartSchoolUser> manager)
    {
        var roles = await manager.GetRolesAsync(user);
        return new UserResponse(user.Id, user.TenantId, user.SchoolId, user.Email ?? string.Empty,
            user.FirstName, user.LastName, user.DisplayName, user.PhoneNumber, user.AccountType,
            user.IsActive, user.MustChangePassword, roles.ToArray());
    }

    private static Dictionary<string,string[]> ToErrors(IdentityResult result) =>
        result.Errors.GroupBy(x=>x.Code).ToDictionary(x=>x.Key,x=>x.Select(e=>e.Description).ToArray());

    private static class TemporaryPasswordGenerator
    {
        public static string Create()
        {
            var random = Convert.ToHexString(RandomNumberGenerator.GetBytes(12));
            return $"Ss!{random}9aA";
        }
    }
}
