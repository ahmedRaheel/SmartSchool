using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using Microsoft.AspNetCore.Identity;
using SmartSchool.Modules.Identity.Infrastructure.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Identity.Server;

/// <summary>
/// Validates the impersonation extension grant.
///
/// Super administrators can impersonate users across tenants.
/// Tenant administrators can impersonate users only within their own tenant.
/// The issued token keeps the original administrator identity in claims
/// for auditing and traceability.
/// </summary>
public sealed class ImpersonationGrantValidator(
	ITokenValidator tokenValidator,
	UserManager<SmartSchoolUser> userManager,
	ILogger<ImpersonationGrantValidator> logger)
	: IExtensionGrantValidator
{
	/// <summary>Gets the Duende extension grant type used for impersonation.</summary>
	public const string GrantTypeName = "impersonation";

	/// <summary>Gets the extension grant type handled by this validator.</summary>
	public string GrantType => GrantTypeName;

	/// <summary>Validates an administrator token and target account before issuing an impersonated subject.</summary>
	public async Task ValidateAsync(
		ExtensionGrantValidationContext context,
		CancellationToken ct)
	{
		var actorToken = context.Request.Raw.Get("actor_token");
		var targetUserId = context.Request.Raw.Get("target_user_id");
		var reason = context.Request.Raw.Get("reason");

		if (string.IsNullOrWhiteSpace(actorToken) ||
			string.IsNullOrWhiteSpace(targetUserId))
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidRequest,
				"actor_token and target_user_id are required.");

			return;
		}

		var actorValidation = await tokenValidator.ValidateAccessTokenAsync(
			actorToken,
			expectedScope: null,
			ct);

		if (actorValidation.IsError ||
			actorValidation.Claims is null)
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidGrant,
				"The administrator token is invalid.");

			return;
		}

		var actorSubject = actorValidation.Claims
			.FirstOrDefault(claim => claim.Type == "sub")
			?.Value;

		if (string.IsNullOrWhiteSpace(actorSubject))
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidGrant,
				"The administrator token does not contain a subject.");

			return;
		}

		var actorRoles = actorValidation.Claims
			.Where(claim => claim.Type == "role")
			.Select(claim => claim.Value)
			.ToHashSet(StringComparer.OrdinalIgnoreCase);

		var actorTenantValue = actorValidation.Claims
			.FirstOrDefault(
				claim => claim.Type == SmartSchoolClaims.TenantId)
			?.Value;

		var isSuperAdmin =
			actorRoles.Contains(SmartSchoolRoles.SuperAdmin);

		var isTenantAdmin =
			actorRoles.Contains(SmartSchoolRoles.SchoolAdmin) ||
			actorRoles.Contains(SmartSchoolRoles.Admin);

		if (!isSuperAdmin && !isTenantAdmin)
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidGrant,
				"The current user cannot impersonate another account.");

			return;
		}

		if (!Guid.TryParse(targetUserId, out var targetId))
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidRequest,
				"target_user_id is invalid.");

			return;
		}

		var target = await userManager.FindByIdAsync(
			targetId.ToString());

		if (target is null ||
			!target.IsActive ||
			IsLockedOut(target))
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidGrant,
				"The target account is unavailable.");

			return;
		}

		if (!isSuperAdmin &&
			!CanTenantAdminImpersonateTarget(
				actorTenantValue,
				target))
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidGrant,
				"Tenant administrators can impersonate users only inside their tenant.");

			return;
		}

		var targetRoles = await userManager.GetRolesAsync(target);

		if (!isSuperAdmin &&
			targetRoles.Contains(
				SmartSchoolRoles.SuperAdmin,
				StringComparer.OrdinalIgnoreCase))
		{
			context.Result = new GrantValidationResult(
				TokenRequestErrors.InvalidGrant,
				"Tenant administrators cannot impersonate a platform administrator.");

			return;
		}

		var claims = BuildImpersonationClaims(
			actorSubject,
			target,
			reason);

		logger.LogWarning(
			"Impersonation token issued. " +
			"Actor={ActorSubject}, Target={TargetUserId}, " +
			"Tenant={TenantId}, Reason={Reason}",
			actorSubject,
			target.Id,
			target.TenantId,
			reason);

		context.Result = new GrantValidationResult(
			subject: target.Id.ToString(),
			authenticationMethod: GrantType,
			claims: claims);
	}

	private static bool IsLockedOut(
		SmartSchoolUser user)
	{
		return user.LockoutEnd.HasValue &&
			   user.LockoutEnd.Value > DateTimeOffset.UtcNow;
	}

	private static bool CanTenantAdminImpersonateTarget(
		string? actorTenantValue,
		SmartSchoolUser target)
	{
		if (!Guid.TryParse(
				actorTenantValue,
				out var actorTenantId))
		{
			return false;
		}

		return target.TenantId == actorTenantId;
	}

	private static IEnumerable<Claim> BuildImpersonationClaims(
		string actorSubject,
		SmartSchoolUser target,
		string? reason)
	{
		var claims = new List<Claim>
		{
			new(
				SmartSchoolClaims.Impersonated,
				"true"),

			new(
				SmartSchoolClaims.ImpersonatorSubject,
				actorSubject),

			new(
				"impersonation_reason",
				reason?.Trim() ?? string.Empty)
		};

		if (target.TenantId.HasValue)
		{
			claims.Add(
				new Claim(
					SmartSchoolClaims.TenantId,
					target.TenantId.Value.ToString()));
		}

		return claims;
	}
}
