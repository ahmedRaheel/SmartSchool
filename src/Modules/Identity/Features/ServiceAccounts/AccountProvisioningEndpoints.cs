using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Persistence.Identity;
using Microsoft.Extensions.Options;

namespace SmartSchool.Modules.Identity.Features.ServiceAccounts;

/// <summary>Internal account lifecycle API called by SmartSchool.Api.</summary>
public static class AccountProvisioningEndpoints
{
    public sealed class AccountProvisioningOptions
    {
        public const string SectionName = "AccountProvisioning";
        public string TemporaryPassword { get; init; } = string.Empty;
    }
    public sealed record ProvisionAccountRequest(
        Guid TenantId, Guid BusinessEntityId, string AccountType, Guid? SchoolId, Guid? BranchId,
        string Email, string FirstName, string LastName, string[] Roles);

    public static void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group=endpoints.MapGroup("/api/internal/accounts")
            .WithTags("Identity - Internal Account Provisioning")
            .RequireAuthorization("SmartSchoolApi");

        group.MapPost("", ProvisionAsync);
        group.MapDelete("/{userId:guid}", DeleteAsync);
        group.MapPost("/{userId:guid}/deactivate", DeactivateAsync);
    }

    private static async Task<IResult> ProvisionAsync(
        ProvisionAccountRequest request,
        UserManager<SmartSchoolUser> users,
        IOptions<AccountProvisioningOptions> provisioningOptions,
        ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("SmartSchool.Identity.AccountProvisioning");
        var existing=await users.FindByEmailAsync(request.Email);
        if(existing is not null) return Results.Conflict(new { message="An account already exists for this email.", userId=existing.Id });

        var user=new SmartSchoolUser
        {
            Id=Guid.NewGuid(), TenantId=request.TenantId, UserName=request.Email, Email=request.Email,
            EmailConfirmed=false, FirstName=request.FirstName, LastName=request.LastName,
            DisplayName=$"{request.FirstName} {request.LastName}".Trim(), IsActive=true,
            BusinessEntityId=request.BusinessEntityId, AccountType=request.AccountType, SchoolId=request.SchoolId, BranchId=request.BranchId,
             MustChangePassword=true
        };

        var temporaryPassword = provisioningOptions.Value.TemporaryPassword;
        if (string.IsNullOrWhiteSpace(temporaryPassword))
            throw new InvalidOperationException("AccountProvisioning:TemporaryPassword configuration is required.");
        var created=await users.CreateAsync(user, temporaryPassword);
        if(!created.Succeeded)
        {
            logger.LogWarning("Provisioning identity user failed for {Email}, tenant {TenantId}, business entity {BusinessEntityId}. Errors: {Errors}",
                request.Email, request.TenantId, request.BusinessEntityId,
                string.Join(", ", created.Errors.Select(error => $"{error.Code}: {error.Description}")));
            return Results.ValidationProblem(Errors(created));
        }

        if(request.Roles.Length>0)
        {
            var roleResult=await users.AddToRolesAsync(user,request.Roles.Distinct(StringComparer.OrdinalIgnoreCase));
            if(!roleResult.Succeeded)
            {
                logger.LogWarning("Provisioned identity user {UserId} but role assignment failed. Roles: {Roles}. Errors: {Errors}",
                    user.Id, request.Roles, string.Join(", ", roleResult.Errors.Select(error => $"{error.Code}: {error.Description}")));
                return Results.ValidationProblem(Errors(roleResult));
            }
        }

        logger.LogInformation("Provisioned identity user {UserId} for tenant {TenantId}, business entity {BusinessEntityId}, account type {AccountType}, roles {Roles}",
            user.Id, user.TenantId, user.BusinessEntityId, user.AccountType, request.Roles);

        // The temporary password is returned only on this provisioning response.
        // ASP.NET Identity persists only the password hash.
        return Results.Created($"/api/identity/users/{user.Id}",
            new
            {
                userId=user.Id,
                email=user.Email!,
                temporaryPassword,
                mustChangePassword=true
            });
    }

    private static async Task<IResult> DeleteAsync(Guid userId, UserManager<SmartSchoolUser> users)
    {
        var user=await users.FindByIdAsync(userId.ToString());
        if(user is null) return Results.NotFound();
        var result=await users.DeleteAsync(user);
        return result.Succeeded ? Results.NoContent() : Results.ValidationProblem(Errors(result));
    }

    private static async Task<IResult> DeactivateAsync(Guid userId, UserManager<SmartSchoolUser> users)
    {
        var user=await users.FindByIdAsync(userId.ToString());
        if(user is null) return Results.NotFound();
        user.IsActive=false; user.UpdatedAt=DateTimeOffset.UtcNow;
        await users.UpdateSecurityStampAsync(user); // invalidates security-stamp-aware sessions/cookies
        var result=await users.UpdateAsync(user);
        return result.Succeeded ? Results.NoContent() : Results.ValidationProblem(Errors(result));
    }

    private static Dictionary<string,string[]> Errors(IdentityResult result) =>
        result.Errors.GroupBy(x=>x.Code).ToDictionary(x=>x.Key,x=>x.Select(e=>e.Description).ToArray());
}
