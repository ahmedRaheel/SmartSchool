using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Identity;

public static class AuthorizationRegistration
{
	public static IServiceCollection AddSmartSchoolAuthorization(
		this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddScoped<ICurrentUser, CurrentUser>();

		services.AddAuthorization(options =>
		{
			AddPolicy(
				options,
				SmartSchoolPolicies.SchoolAdministration,
				SmartSchoolRoles.SuperAdmin,
				SmartSchoolRoles.SchoolAdmin,
				SmartSchoolRoles.Principal);

			AddPolicy(
				options,
				SmartSchoolPolicies.AcademicManagement,
				SmartSchoolRoles.SchoolAdmin,
				SmartSchoolRoles.Principal,
				SmartSchoolRoles.Teacher);

			AddPolicy(
				options,
				SmartSchoolPolicies.StudentSelfService,
				SmartSchoolRoles.StudentEntity);

			AddPolicy(
				options,
				SmartSchoolPolicies.ParentSelfService,
				SmartSchoolRoles.Parent);

			AddPolicy(
				options,
				SmartSchoolPolicies.FinanceManagement,
				SmartSchoolRoles.SchoolAdmin,
				SmartSchoolRoles.Accountant);

			AddPolicy(
				options,
				SmartSchoolPolicies.HumanResourcesManagement,
				SmartSchoolRoles.SchoolAdmin,
				SmartSchoolRoles.HrManager);
		});

		return services;
	}

	private static void AddPolicy(
		AuthorizationOptions options,
		string policyName,
		params string[] roles)
	{
		options.AddPolicy(
			policyName,
			policy =>
			{
				policy.RequireAuthenticatedUser();
				policy.RequireRole(roles);
			});
	}
}
