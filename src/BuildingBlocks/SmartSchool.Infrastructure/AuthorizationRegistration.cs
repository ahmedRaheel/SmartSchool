using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Identity;

public static class AuthorizationRegistration
{
    public static IServiceCollection AddSmartSchoolAuthorization(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();

        services.AddAuthorization(options =>
        {
            AddPolicy(options, SmartSchoolPolicies.PlatformAdministration, SmartSchoolRoles.SuperAdmin);
            AddPolicy(options, SmartSchoolPolicies.SchoolAdministration,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Principal);
            AddPolicy(options, SmartSchoolPolicies.AcademicManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Principal, SmartSchoolRoles.Teacher);
            AddPolicy(options, SmartSchoolPolicies.TeacherWorkspace, SmartSchoolRoles.Teacher);
            AddPolicy(options, SmartSchoolPolicies.StudentSelfService, SmartSchoolRoles.Student);
            AddPolicy(options, SmartSchoolPolicies.ParentSelfService, SmartSchoolRoles.Parent);
            AddPolicy(options, SmartSchoolPolicies.DriverWorkspace, SmartSchoolRoles.Driver);
            AddPolicy(options, SmartSchoolPolicies.ExaminationManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Principal, SmartSchoolRoles.Examiner);
            AddPolicy(options, SmartSchoolPolicies.FinanceManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Accountant);
            AddPolicy(options, SmartSchoolPolicies.HumanResourcesManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.HrManager);
            options.AddPolicy("SuperAdminOnly", policy =>
                policy.RequireAuthenticatedUser().RequireRole(SmartSchoolRoles.SuperAdmin));
        });
        return services;
    }

    private static void AddPolicy(AuthorizationOptions options, string name, params string[] roles) =>
        options.AddPolicy(name, policy => policy.RequireAuthenticatedUser().RequireRole(roles));
}
