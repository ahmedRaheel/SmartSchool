using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Modules.Identity.Server;

public sealed class SmartSchoolProfileService(
	UserManager<SmartSchoolUser> userManager) : IProfileService
{
	public Task GetProfileDataAsync(ProfileDataRequestContext context) =>
		GetProfileDataAsync(context, CancellationToken.None);

	public async Task GetProfileDataAsync(ProfileDataRequestContext context, CancellationToken ct)
	{
		var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());
		if (user is null || !user.IsActive) return;

		var roles = await userManager.GetRolesAsync(user);
		var claims = new List<Claim>
		{
			new("tenant_id", user.TenantId?.ToString() ?? string.Empty),
			new("given_name", user.FirstName),
			new("family_name", user.LastName),
			new("name", user.DisplayName ?? $"{user.FirstName} {user.LastName}".Trim()),
			new("email", user.Email ?? string.Empty),
			new("account_type", user.AccountType ?? string.Empty)
		};
		claims.AddRange(roles.Select(role => new Claim("role", role)));
		context.IssuedClaims.AddRange(claims);
	}

	public Task IsActiveAsync(IsActiveContext context) =>
		IsActiveAsync(context, CancellationToken.None);

	public async Task IsActiveAsync(IsActiveContext context, CancellationToken ct)
	{
		var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());
		context.IsActive = user is not null && user.IsActive &&
			(!user.LockoutEnd.HasValue || user.LockoutEnd.Value <= DateTimeOffset.UtcNow);
	}
}
