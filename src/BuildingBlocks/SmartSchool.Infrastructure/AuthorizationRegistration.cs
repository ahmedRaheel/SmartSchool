using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Identity;

namespace SmartSchool.Infrastructure.Identity;

public static class AuthorizationRegistration
{
    public static IServiceCollection AddSmartSchoolAuthorization(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<ITenantScope, TenantScope>();

        services.AddAuthorization(options =>
        {
            AddPolicy(options, SmartSchoolPolicies.PlatformAdministration, SmartSchoolRoles.SuperAdmin);
            AddPolicy(options, SmartSchoolPolicies.UserAdministration, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin);
            AddPolicy(options, SmartSchoolPolicies.Impersonation, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin);
            AddPolicy(options, SmartSchoolPolicies.WorkflowAdministration, SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Principal);
            AddPolicy(options, SmartSchoolPolicies.SchoolAdministration,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Principal);
            AddPolicy(options, SmartSchoolPolicies.AcademicManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Principal, SmartSchoolRoles.Teacher);
            AddPolicy(options, SmartSchoolPolicies.TeacherWorkspace, SmartSchoolRoles.Teacher, SmartSchoolRoles.SuperAdmin);
            AddPolicy(options, SmartSchoolPolicies.StudentSelfService, SmartSchoolRoles.Student, SmartSchoolRoles.SuperAdmin);
            AddPolicy(options, SmartSchoolPolicies.ParentSelfService, SmartSchoolRoles.Parent, SmartSchoolRoles.SuperAdmin);
            AddPolicy(options, SmartSchoolPolicies.DriverWorkspace, SmartSchoolRoles.Driver, SmartSchoolRoles.SuperAdmin);
            AddPolicy(options, SmartSchoolPolicies.ExaminationManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Principal, SmartSchoolRoles.Examiner);
            AddPolicy(options, SmartSchoolPolicies.FinanceManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Accountant);
            AddPolicy(options, SmartSchoolPolicies.HumanResourcesManagement,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.HrManager);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminOnly,
                SmartSchoolRoles.SuperAdmin);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantOnly,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantTeacher,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Teacher);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantStudent,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Student);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantParent,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Parent);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantAdmin,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantDriver,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Driver);
            AddPolicy(options, SmartSchoolPolicies.AllAuthenticatedActors,
                SmartSchoolRoles.SuperAdmin, SmartSchoolRoles.SchoolAdmin, SmartSchoolRoles.Admin, SmartSchoolRoles.Principal,
                SmartSchoolRoles.Teacher, SmartSchoolRoles.Student, SmartSchoolRoles.Parent,
                SmartSchoolRoles.Driver, SmartSchoolRoles.Examiner, SmartSchoolRoles.Staff,
                SmartSchoolRoles.Accountant, SmartSchoolRoles.HrManager, SmartSchoolRoles.Librarian,
                SmartSchoolRoles.TransportManager, SmartSchoolRoles.AdmissionOfficer);
        });
        return services;
    }

    private static void AddPolicy(AuthorizationOptions options, string name, params string[] roles) =>
        options.AddPolicy(name, policy => policy.RequireAuthenticatedUser().RequireRole(roles));
}
