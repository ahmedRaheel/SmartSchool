using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Modules.Identity.Features.Users;

public static class UserManagementEndpoints
{
	public sealed record CreateUserRequest(Guid TenantId, string Email, string Password, string FirstName, string LastName, string[] Roles);
	public sealed record UpdateUserRequest(string FirstName, string LastName, string? DisplayName, string? PhoneNumber, bool IsActive);
	public sealed record ResetPasswordRequest(string NewPassword);
	public sealed record SetRolesRequest(string[] Roles);
	public sealed record UserResponse(Guid Id, Guid TenantId, string Email, string FirstName, string LastName, string? DisplayName, string? PhoneNumber, bool IsActive, IReadOnlyList<string> Roles);

	public static void MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/api/identity/users")
			.WithTags("Identity - Users")
			.RequireAuthorization("AdminOnly");

		group.MapGet("", GetPageAsync);
		group.MapGet("/{id:guid}", GetByIdAsync);
		group.MapPost("", CreateAsync);
		group.MapPut("/{id:guid}", UpdateAsync);
		group.MapPut("/{id:guid}/roles", SetRolesAsync);
		group.MapPost("/{id:guid}/reset-password", ResetPasswordAsync);
		group.MapPost("/{id:guid}/lock", LockAsync);
		group.MapPost("/{id:guid}/unlock", UnlockAsync);
		group.MapDelete("/{id:guid}", DeactivateAsync);
	}

	private static async Task<IResult> GetPageAsync(int page, int pageSize, Guid? tenantId,
		UserManager<SmartSchoolUser> userManager, CancellationToken cancellationToken)
	{
		page = Math.Max(1, page);
		pageSize = Math.Clamp(pageSize <= 0 ? 20 : pageSize, 1, 100);
		var query = userManager.Users.AsNoTracking();
		if (tenantId.HasValue) query = query.Where(x => x.TenantId == tenantId.Value);
		var total = await query.CountAsync(cancellationToken);
		var users = await query.OrderBy(x => x.Email).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
		var result = new List<UserResponse>(users.Count);
		foreach (var user in users) result.Add(await ToResponseAsync(user, userManager));
		return Results.Ok(new { page, pageSize, total, items = result });
	}

	private static async Task<IResult> GetByIdAsync(Guid id, UserManager<SmartSchoolUser> userManager)
	{
		var user = await userManager.FindByIdAsync(id.ToString());
		return user is null ? Results.NotFound() : Results.Ok(await ToResponseAsync(user, userManager));
	}

	private static async Task<IResult> CreateAsync(CreateUserRequest request, UserManager<SmartSchoolUser> userManager)
	{
		var user = new SmartSchoolUser
		{
			Id = Guid.NewGuid(), TenantId = request.TenantId, UserName = request.Email,
			Email = request.Email, FirstName = request.FirstName, LastName = request.LastName,
			DisplayName = $"{request.FirstName} {request.LastName}".Trim(), EmailConfirmed = true
		};
		var result = await userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded) return Results.ValidationProblem(ToErrors(result));
		if (request.Roles.Length > 0)
		{
			var roleResult = await userManager.AddToRolesAsync(user, request.Roles.Distinct(StringComparer.OrdinalIgnoreCase));
			if (!roleResult.Succeeded) return Results.ValidationProblem(ToErrors(roleResult));
		}
		return Results.Created($"/api/identity/users/{user.Id}", await ToResponseAsync(user, userManager));
	}

	private static async Task<IResult> UpdateAsync(Guid id, UpdateUserRequest request, UserManager<SmartSchoolUser> userManager)
	{
		var user = await userManager.FindByIdAsync(id.ToString());
		if (user is null) return Results.NotFound();
		user.FirstName=request.FirstName; user.LastName=request.LastName; user.DisplayName=request.DisplayName;
		user.PhoneNumber=request.PhoneNumber; user.IsActive=request.IsActive; user.UpdatedAt=DateTimeOffset.UtcNow;
		var result=await userManager.UpdateAsync(user);
		return result.Succeeded ? Results.Ok(await ToResponseAsync(user,userManager)) : Results.ValidationProblem(ToErrors(result));
	}

	private static async Task<IResult> SetRolesAsync(Guid id, SetRolesRequest request, UserManager<SmartSchoolUser> userManager)
	{
		var user=await userManager.FindByIdAsync(id.ToString());
		if(user is null) return Results.NotFound();
		var current=await userManager.GetRolesAsync(user);
		var remove=await userManager.RemoveFromRolesAsync(user,current);
		if(!remove.Succeeded) return Results.ValidationProblem(ToErrors(remove));
		var add=await userManager.AddToRolesAsync(user,request.Roles.Distinct(StringComparer.OrdinalIgnoreCase));
		return add.Succeeded ? Results.Ok(await ToResponseAsync(user,userManager)) : Results.ValidationProblem(ToErrors(add));
	}

	private static async Task<IResult> ResetPasswordAsync(Guid id, ResetPasswordRequest request, UserManager<SmartSchoolUser> userManager)
	{
		var user=await userManager.FindByIdAsync(id.ToString());
		if(user is null) return Results.NotFound();
		var token=await userManager.GeneratePasswordResetTokenAsync(user);
		var result=await userManager.ResetPasswordAsync(user,token,request.NewPassword);
		return result.Succeeded ? Results.NoContent() : Results.ValidationProblem(ToErrors(result));
	}

	private static async Task<IResult> LockAsync(Guid id, UserManager<SmartSchoolUser> userManager)
	{
		var user=await userManager.FindByIdAsync(id.ToString());
		if(user is null) return Results.NotFound();
		await userManager.SetLockoutEndDateAsync(user,DateTimeOffset.UtcNow.AddYears(100));
		return Results.NoContent();
	}
	private static async Task<IResult> UnlockAsync(Guid id, UserManager<SmartSchoolUser> userManager)
	{
		var user=await userManager.FindByIdAsync(id.ToString());
		if(user is null) return Results.NotFound();
		await userManager.SetLockoutEndDateAsync(user,null);
		await userManager.ResetAccessFailedCountAsync(user);
		return Results.NoContent();
	}
	private static async Task<IResult> DeactivateAsync(Guid id, UserManager<SmartSchoolUser> userManager)
	{
		var user=await userManager.FindByIdAsync(id.ToString());
		if(user is null) return Results.NotFound();
		user.IsActive=false; user.UpdatedAt=DateTimeOffset.UtcNow;
		await userManager.UpdateAsync(user);
		await userManager.GetRolesAsync(user);
		return Results.NoContent();
	}
	private static async Task<UserResponse> ToResponseAsync(
	SmartSchoolUser user,
	UserManager<SmartSchoolUser> manager)
	{
		var roles = await manager.GetRolesAsync(user);

		return new UserResponse(
			user.Id,
			user.TenantId  ?? Guid.Empty,
			user.Email ?? string.Empty,
			user.FirstName,
			user.LastName,
			user.DisplayName,
			user.PhoneNumber,
			user.IsActive,
			roles.ToArray());
	}
	private static Dictionary<string,string[]> ToErrors(IdentityResult result) =>
		result.Errors.GroupBy(x=>x.Code).ToDictionary(x=>x.Key,x=>x.Select(e=>e.Description).ToArray());
}
