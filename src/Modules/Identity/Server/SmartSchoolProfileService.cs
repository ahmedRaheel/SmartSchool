using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Infrastructure.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Server;

public sealed class SmartSchoolProfileService(
    UserManager<SmartSchoolUser> userManager) : IProfileService
{
    public Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        return GetProfileDataAsync(context, CancellationToken.None);
    }

    public async Task GetProfileDataAsync(
        ProfileDataRequestContext context,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());

        if (user is null || !user.IsActive)
        {
            return;
        }

        var roles = await userManager.GetRolesAsync(user);
		if (roles is null)
		{
			return;
		}
        var claims = BuildUserClaims(user, roles.ToList());

        context.IssuedClaims.AddRange(claims);
    }

    public Task IsActiveAsync(IsActiveContext context)
    {
        return IsActiveAsync(context, CancellationToken.None);
    }

    public async Task IsActiveAsync(
        IsActiveContext context,
        CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());

        context.IsActive = user is not null &&
                           user.IsActive &&
                           (!user.LockoutEnd.HasValue ||
                            user.LockoutEnd.Value <= DateTimeOffset.UtcNow);
    }

    private static IReadOnlyCollection<Claim> BuildUserClaims(
        SmartSchoolUser user,
        IReadOnlyCollection<string> roles)
    {
        var claims = new List<Claim>
        {
            new(SmartSchoolClaims.FirstName, user.FirstName),
            new(SmartSchoolClaims.LastName, user.LastName),
            new(
                SmartSchoolClaims.DisplayName,
                user.DisplayName ?? $"{user.FirstName} {user.LastName}".Trim()),
            new(
                SmartSchoolClaims.MustChangePassword,
                user.MustChangePassword ? "true" : "false")
        };

        AddOptionalClaim(claims, SmartSchoolClaims.TenantId, user.TenantId);
        AddOptionalClaim(claims, SmartSchoolClaims.SchoolId, user.SchoolId);
        AddOptionalClaim(claims, SmartSchoolClaims.BranchId, user.BranchId);
        AddOptionalClaim(claims, SmartSchoolClaims.Email, user.Email);
        AddOptionalClaim(claims, SmartSchoolClaims.AccountType, user.AccountType);
        AddActorClaim(claims, user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(SmartSchoolClaims.Role, role));
        }

        return claims;
    }

    private static void AddActorClaim(
        ICollection<Claim> claims,
        SmartSchoolUser user)
    {
        if (!user.BusinessEntityId.HasValue)
        {
            return;
        }

        var claimType = GetActorClaimType(user.AccountType);

        claims.Add(
            new Claim(
                claimType,
                user.BusinessEntityId.Value.ToString()));
    }

    private static string GetActorClaimType(string? accountType)
    {
        return accountType?.Trim().ToLowerInvariant() switch
        {
            "student" => SmartSchoolClaims.StudentId,
            "teacher" => SmartSchoolClaims.TeacherId,
            "driver" => SmartSchoolClaims.DriverId,
            "examiner" => SmartSchoolClaims.ExaminerId,
            _ => SmartSchoolClaims.EmployeeId
        };
    }

    private static void AddOptionalClaim(
        ICollection<Claim> claims,
        string claimType,
        Guid? value)
    {
        if (value.HasValue)
        {
            claims.Add(new Claim(claimType, value.Value.ToString()));
        }
    }

    private static void AddOptionalClaim(
        ICollection<Claim> claims,
        string claimType,
        string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            claims.Add(new Claim(claimType, value));
        }
    }
}
