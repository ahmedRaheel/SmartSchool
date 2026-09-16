using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Infrastructure.Identity;

public static class AuthorizationRegistration
{
    private static readonly string[] PlatformRoles =
    [
        SmartSchoolRoles.SuperAdmin,
        SmartSchoolRoles.SuperOwner
    ];

    private static readonly string[] TenantAdministrationRoles =
    [
        SmartSchoolRoles.SuperAdmin,
        SmartSchoolRoles.SuperOwner,
        SmartSchoolRoles.Tenant,
        SmartSchoolRoles.TenantAdmin,
        SmartSchoolRoles.Owner,
        SmartSchoolRoles.Admin,
        SmartSchoolRoles.AdminOfficer
    ];

    public static IServiceCollection AddSmartSchoolAuthorization(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<Application.Identity.ICurrentUser, Application.Identity.CurrentUser>();
        services.AddScoped<ITenantScope, Application.Identity.TenantScope>();

        services.AddAuthorization(options =>
        {
            AddPolicy(options, SmartSchoolPolicies.PlatformAdministration, PlatformRoles);
            AddPolicy(options, SmartSchoolPolicies.UserAdministration, TenantAdministrationRoles);
            AddPolicy(options, SmartSchoolPolicies.Impersonation, TenantAdministrationRoles);
            AddPolicy(options, SmartSchoolPolicies.WorkflowAdministration,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Principal]);
            AddPolicy(options, SmartSchoolPolicies.SchoolAdministration,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Principal]);
            AddPolicy(options, SmartSchoolPolicies.AcademicManagement,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Principal, SmartSchoolRoles.Teacher]);
            AddPolicy(options, SmartSchoolPolicies.TeacherWorkspace,
                [SmartSchoolRoles.Teacher, SmartSchoolRoles.Principal, .. PlatformRoles]);
            AddPolicy(options, SmartSchoolPolicies.StudentSelfService,
                [SmartSchoolRoles.Student, .. PlatformRoles]);
            AddPolicy(options, SmartSchoolPolicies.ParentSelfService,
                [SmartSchoolRoles.Parent, .. PlatformRoles]);
            AddPolicy(options, SmartSchoolPolicies.DriverWorkspace,
                [SmartSchoolRoles.Driver, SmartSchoolRoles.Admin, SmartSchoolRoles.AdminOfficer, .. PlatformRoles]);
            AddPolicy(options, SmartSchoolPolicies.ExaminationManagement,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Principal, SmartSchoolRoles.Examiner]);
            AddPolicy(options, SmartSchoolPolicies.FinanceManagement,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Accountant, SmartSchoolRoles.FinanceOfficer]);
            AddPolicy(options, SmartSchoolPolicies.HumanResourcesManagement,
                [.. TenantAdministrationRoles, SmartSchoolRoles.HrManager, SmartSchoolRoles.HR]);
            AddPolicy(options, SmartSchoolPolicies.AiKnowledgeContribution,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Principal, SmartSchoolRoles.Teacher,
                 SmartSchoolRoles.Examiner, SmartSchoolRoles.HrManager, SmartSchoolRoles.HR,
                 SmartSchoolRoles.Accountant, SmartSchoolRoles.FinanceOfficer]);

            AddPolicy(options, SmartSchoolPolicies.SuperAdminOnly, PlatformRoles);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantOnly, TenantAdministrationRoles);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantTeacher,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Teacher]);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantStudent,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Student]);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantParent,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Parent]);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantAdmin, TenantAdministrationRoles);
            AddPolicy(options, SmartSchoolPolicies.SuperAdminTenantDriver,
                [.. TenantAdministrationRoles, SmartSchoolRoles.Driver]);
            AddPolicy(options, SmartSchoolPolicies.AllAuthenticatedActors,
                [.. TenantAdministrationRoles,
                 SmartSchoolRoles.Principal, SmartSchoolRoles.Teacher, SmartSchoolRoles.Student,
                 SmartSchoolRoles.Parent, SmartSchoolRoles.Driver, SmartSchoolRoles.Examiner,
                 SmartSchoolRoles.Accountant, SmartSchoolRoles.FinanceOfficer,
                 SmartSchoolRoles.HrManager, SmartSchoolRoles.HR, SmartSchoolRoles.Librarian]);
        });

        return services;
    }

    private static void AddPolicy(AuthorizationOptions options, string name, params string[] roles) =>
        options.AddPolicy(name, policy => policy.RequireAuthenticatedUser().RequireRole(roles));
}
