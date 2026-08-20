using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Modules.Identity.Features.ServiceAccounts;

/// <summary>Internal account lifecycle API called by SmartSchool.Api.</summary>
public static class AccountProvisioningEndpoints
{
	public sealed record ProvisionAccountRequest(
		Guid TenantId, Guid BusinessEntityId, string AccountType,
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
		UserManager<SmartSchoolUser> users)
	{
		var existing=await users.FindByEmailAsync(request.Email);
		if(existing is not null) return Results.Conflict(new { message="An account already exists for this email.", userId=existing.Id });

		var user=new SmartSchoolUser
		{
			Id=Guid.NewGuid(), TenantId=request.TenantId, UserName=request.Email, Email=request.Email,
			EmailConfirmed=false, FirstName=request.FirstName, LastName=request.LastName,
			DisplayName=$"{request.FirstName} {request.LastName}".Trim(), IsActive=true,
			BusinessEntityId=request.BusinessEntityId, AccountType=request.AccountType
		};

		// No default password is created. The user completes password setup through a reset/invitation token.
		var created=await users.CreateAsync(user);
		if(!created.Succeeded) return Results.ValidationProblem(Errors(created));

		if(request.Roles.Length>0)
		{
			var roleResult=await users.AddToRolesAsync(user,request.Roles.Distinct(StringComparer.OrdinalIgnoreCase));
			if(!roleResult.Succeeded) return Results.ValidationProblem(Errors(roleResult));
		}

		var setupToken=await users.GeneratePasswordResetTokenAsync(user);
		return Results.Created($"/api/identity/users/{user.Id}",
			new { userId=user.Id, setupToken });
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
