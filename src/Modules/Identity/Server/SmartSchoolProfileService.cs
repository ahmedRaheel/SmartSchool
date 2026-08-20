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
	public async Task GetProfileDataAsync(ProfileDataRequestContext context)
	{
		var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());
		if (user is null)
		{
			return;
		}

		var roles = await userManager.GetRolesAsync(user);
		var claims = new List<Claim>
		{
			new("tenant_id", user.TenantId?.ToString() ?? string.Empty),
			new("given_name", user.FirstName),
			new("family_name", user.LastName),
			new("name", user.DisplayName ?? $"{user.FirstName} {user.LastName}".Trim()),
			new("email", user.Email ?? string.Empty)
		};
		claims.AddRange(roles.Select(role => new Claim("role", role)));
		context.IssuedClaims.AddRange(claims);
	}

	public Task GetProfileDataAsync(ProfileDataRequestContext context, CancellationToken ct)
	{
		throw new NotImplementedException();
	}

	public async Task IsActiveAsync(IsActiveContext context)
	{
		var user = await userManager.FindByIdAsync(context.Subject.GetSubjectId());
		context.IsActive = user is not null && user.IsActive && !user.LockoutEnd.HasValue;
	}

	public Task IsActiveAsync(IsActiveContext context, CancellationToken ct)
	{
		throw new NotImplementedException();
	}
}
