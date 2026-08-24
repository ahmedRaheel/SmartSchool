using SmartSchool.SharedKernel.Constants;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Identity.Persistence.Identity;
using SmartSchool.Modules.Identity.Server;

namespace SmartSchool.Modules.Identity.Features.Users;

public static class UserManagementEndpoints
{
    public sealed record CreateUserRequest(
        Guid TenantId, Guid? SchoolId, string Email, string? Password,
        string FirstName, string LastName, string AccountType, string[] Roles);

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
        new(StringComparer.OrdinalIgnoreCase)
        { "SchoolAdmin", "Admin", "Principal", "Teacher", "Parent", "Student", "Staff", "Driver" };

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

        // Platform-only operations.
        group.MapPost("/tenant/{tenantId:guid}/status", SetTenantStatusAsync)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);
        group.MapDelete("/tenant/{tenantId:guid}", DeleteTenantUsersAsync)
            .RequireAuthorization(SmartSchoolPolicies.SuperAdminOnly);
        group.MapPost("/impersonation/start", StartImpersonationAsync)
            .RequireAuthorization(SmartSchoolPolicies.Impersonation);
    }

    private static bool IsSuperAdmin(ClaimsPrincipal principal) =>
        principal.IsInRole("SuperAdmin");

    private static Guid? CurrentTenant(ClaimsPrincipal principal) =>
        Guid.TryParse(principal.FindFirst("tenant_id")?.Value, out var id) ? id : null;

    private static async Task<IResult> GetPageAsync(
        int page, int pageSize, Guid? tenantId, ClaimsPrincipal principal,
        UserManager<SmartSchoolUser> userManager, CancellationToken cancellationToken)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize <= 0 ? 20 : pageSize, 1, 100);

        var effectiveTenant = IsSuperAdmin(principal) ? tenantId : CurrentTenant(principal);
        if (!IsSuperAdmin(principal) && effectiveTenant is null) return Results.Forbid();

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
        Guid id, ClaimsPrincipal principal, UserManager<SmartSchoolUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return Results.NotFound();
        if (!IsSuperAdmin(principal) && user.TenantId != CurrentTenant(principal)) return Results.Forbid();
        return Results.Ok(await ToResponseAsync(user, userManager));
    }

    private static async Task<IResult> CreateAsync(
        CreateUserRequest request, ClaimsPrincipal principal,
        UserManager<SmartSchoolUser> userManager,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("SmartSchool.Identity.UserManagement");
        var superAdmin = IsSuperAdmin(principal);
        var callerTenant = CurrentTenant(principal);

        if (!superAdmin && callerTenant != request.TenantId) return Results.Forbid();

        var requestedRoles = request.Roles.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
        if (!superAdmin && requestedRoles.Any(r => !SchoolRoles.Contains(r) || r.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase)))
            return Results.Forbid();

        var password = string.IsNullOrWhiteSpace(request.Password)
            ? TemporaryPasswordGenerator.Create()
            : request.Password;

        var user = new SmartSchoolUser
        {
            Id = Guid.NewGuid(),
            TenantId = request.TenantId,
            SchoolId = request.SchoolId,
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
        ChangePasswordRequest request, ClaimsPrincipal principal,
        UserManager<SmartSchoolUser> userManager)
    {
        var id = principal.FindFirst("sub")?.Value ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(id)) return Results.Unauthorized();
        var user = await userManager.FindByIdAsync(id);
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
        UserManager<SmartSchoolUser> userManager, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.Where(x => x.TenantId == tenantId).ToListAsync(cancellationToken);
        foreach (var user in users)
        {
            user.IsActive = request.IsActive;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            await userManager.UpdateAsync(user);
            if (!request.IsActive) await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddYears(100));
            else
            {
                await userManager.SetLockoutEndDateAsync(user, null);
                await userManager.ResetAccessFailedCountAsync(user);
            }
        }
        return Results.Ok(new { tenantId, isActive = request.IsActive, affectedUsers = users.Count });
    }

    private static async Task<IResult> DeleteTenantUsersAsync(
        Guid tenantId, UserManager<SmartSchoolUser> userManager, CancellationToken cancellationToken)
    {
        var users = await userManager.Users.Where(x => x.TenantId == tenantId).ToListAsync(cancellationToken);
        foreach (var user in users)
        {
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return Results.ValidationProblem(ToErrors(result));
        }
        return Results.Ok(new { tenantId, deletedUsers = users.Count });
    }

    // This endpoint creates an audited support intent. Token exchange is intentionally handled by
    // IdentityServer, not by revealing or resetting the target user's password.
    private static async Task<IResult> StartImpersonationAsync(
        ImpersonateRequest request, ClaimsPrincipal principal,
        UserManager<SmartSchoolUser> userManager, ILoggerFactory loggerFactory)
    {
        var target = await userManager.FindByIdAsync(request.TargetUserId.ToString());
        if (target is null || !target.IsActive) return Results.NotFound();

        var isSuperAdmin = IsSuperAdmin(principal);
        var callerTenant = CurrentTenant(principal);
        if (!isSuperAdmin && (!callerTenant.HasValue || target.TenantId != callerTenant.Value))
            return Results.Forbid();

        var roles = await userManager.GetRolesAsync(target);
        if (!isSuperAdmin && roles.Contains(SmartSchoolRoles.SuperAdmin, StringComparer.OrdinalIgnoreCase))
            return Results.Forbid();
        var impersonatorId = principal.FindFirst("sub")?.Value;
        loggerFactory.CreateLogger("SmartSchool.Impersonation").LogWarning(
            "SuperAdmin {ImpersonatorId} started support impersonation for {TargetUserId} tenant {TenantId}. Reason: {Reason}",
            impersonatorId, target.Id, target.TenantId, request.Reason);

        return Results.Ok(new
        {
            targetUser = await ToResponseAsync(target, userManager),
            impersonation = new
            {
                targetUserId = target.Id,
                target.TenantId,
                target.SchoolId,
                roles,
                impersonatorId,
                request.Reason,
                startedAtUtc = DateTimeOffset.UtcNow
            },
            requiresTokenExchange = true,
            grantType = ImpersonationGrantValidator.GrantTypeName,
            tokenEndpoint = "/connect/token",
            tokenParameters = new[] { "actor_token", "target_user_id", "reason" }
        });
    }

    private static async Task<IResult> UpdateAsync(Guid id, UpdateUserRequest request, ClaimsPrincipal principal, UserManager<SmartSchoolUser> userManager)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null) return Results.NotFound();
        if (!IsSuperAdmin(principal) && user.TenantId != CurrentTenant(principal)) return Results.Forbid();
        user.FirstName=request.FirstName; user.LastName=request.LastName; user.DisplayName=request.DisplayName;
        user.PhoneNumber=request.PhoneNumber; user.IsActive=request.IsActive; user.UpdatedAt=DateTimeOffset.UtcNow;
        var result=await userManager.UpdateAsync(user);
        return result.Succeeded ? Results.Ok(await ToResponseAsync(user,userManager)) : Results.ValidationProblem(ToErrors(result));
    }

    private static async Task<IResult> SetRolesAsync(Guid id, SetRolesRequest request, ClaimsPrincipal principal, UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!IsSuperAdmin(principal) && user.TenantId != CurrentTenant(principal)) return Results.Forbid();
        if (!IsSuperAdmin(principal) && request.Roles.Any(r => !SchoolRoles.Contains(r) || r.Equals("SuperAdmin", StringComparison.OrdinalIgnoreCase))) return Results.Forbid();
        var current=await userManager.GetRolesAsync(user);
        var remove=await userManager.RemoveFromRolesAsync(user,current);
        if(!remove.Succeeded) return Results.ValidationProblem(ToErrors(remove));
        var add=await userManager.AddToRolesAsync(user,request.Roles.Distinct(StringComparer.OrdinalIgnoreCase));
        return add.Succeeded ? Results.Ok(await ToResponseAsync(user,userManager)) : Results.ValidationProblem(ToErrors(add));
    }

    private static async Task<IResult> ResetPasswordAsync(Guid id, ResetPasswordRequest request, ClaimsPrincipal principal, UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!IsSuperAdmin(principal) && user.TenantId != CurrentTenant(principal)) return Results.Forbid();
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

    private static async Task<IResult> LockAsync(Guid id, ClaimsPrincipal principal, UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!IsSuperAdmin(principal) && user.TenantId != CurrentTenant(principal)) return Results.Forbid();
        await userManager.SetLockoutEndDateAsync(user,DateTimeOffset.UtcNow.AddYears(100));
        return Results.NoContent();
    }

    private static async Task<IResult> UnlockAsync(Guid id, ClaimsPrincipal principal, UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!IsSuperAdmin(principal) && user.TenantId != CurrentTenant(principal)) return Results.Forbid();
        await userManager.SetLockoutEndDateAsync(user,null);
        await userManager.ResetAccessFailedCountAsync(user);
        return Results.NoContent();
    }

    private static async Task<IResult> DeactivateAsync(Guid id, ClaimsPrincipal principal, UserManager<SmartSchoolUser> userManager)
    {
        var user=await userManager.FindByIdAsync(id.ToString());
        if(user is null) return Results.NotFound();
        if (!IsSuperAdmin(principal) && user.TenantId != CurrentTenant(principal)) return Results.Forbid();
        user.IsActive=false; user.UpdatedAt=DateTimeOffset.UtcNow;
        await userManager.UpdateAsync(user);
        return Results.NoContent();
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
        public static string Create() =>
            $"Ss!{Guid.NewGuid():N}"[..14] + "9aA";
    }
}
