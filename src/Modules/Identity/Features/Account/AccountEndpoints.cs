using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Persistence.Identity;

namespace SmartSchool.Modules.Identity.Features.Account;

public static class AccountEndpoints
{
	public sealed record ForgotPasswordRequest(string Email);
	public sealed record ResetPasswordRequest(string Email, string Token, string NewPassword);
	public sealed record ChangePasswordRequest(string CurrentPassword, string NewPassword);

	public static void MapEndpoints(IEndpointRouteBuilder endpoints)
	{
		var group = endpoints.MapGroup("/api/account").WithTags("Identity - Account");

		group.MapPost("/forgot-password", ForgotPasswordAsync).AllowAnonymous();
		group.MapPost("/reset-password", ResetPasswordAsync).AllowAnonymous();
		group.MapPost("/change-password", ChangePasswordAsync).RequireAuthorization();
	}

	private static async Task<IResult> ForgotPasswordAsync(
		ForgotPasswordRequest request,
		UserManager<SmartSchoolUser> userManager,
		ILoggerFactory loggerFactory)
	{
		// Always return the same response to prevent account enumeration.
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user is not null && user.IsActive)
		{
			var token = await userManager.GeneratePasswordResetTokenAsync(user);
			// Development fallback only. Replace with SmartSchool notification/email sender in production.
			loggerFactory.CreateLogger("PasswordReset")
				.LogInformation("Password reset requested for user {UserId}. Token generated: {Token}", user.Id, token);
		}

		return Results.Accepted(value: new
		{
			message = "If the account exists, password reset instructions will be sent."
		});
	}

	private static async Task<IResult> ResetPasswordAsync(
		ResetPasswordRequest request,
		UserManager<SmartSchoolUser> userManager)
	{
		var user = await userManager.FindByEmailAsync(request.Email);
		if (user is null || !user.IsActive)
		{
			return Results.BadRequest(new { message = "Invalid password reset request." });
		}

		var result = await userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
		return result.Succeeded
			? Results.NoContent()
			: Results.ValidationProblem(ToErrors(result));
	}

	private static async Task<IResult> ChangePasswordAsync(
		ChangePasswordRequest request,
		System.Security.Claims.ClaimsPrincipal principal,
		UserManager<SmartSchoolUser> userManager)
	{
		var user = await userManager.GetUserAsync(principal);
		if (user is null) return Results.Unauthorized();

		var result = await userManager.ChangePasswordAsync(
			user, request.CurrentPassword, request.NewPassword);

		return result.Succeeded
			? Results.NoContent()
			: Results.ValidationProblem(ToErrors(result));
	}

	private static Dictionary<string, string[]> ToErrors(IdentityResult result) =>
		result.Errors.GroupBy(x => x.Code)
			.ToDictionary(x => x.Key, x => x.Select(e => e.Description).ToArray());
}
