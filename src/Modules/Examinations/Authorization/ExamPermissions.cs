using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;
namespace SmartSchool.Modules.Examinations.Authorization;
public static class ExamPermissions
{
    public static bool CanManage(ICurrentUser user) => user.IsSuperAdmin || new[] {
        SmartSchoolRoles.Tenant, SmartSchoolRoles.TenantAdmin, SmartSchoolRoles.Owner,
        SmartSchoolRoles.Admin, SmartSchoolRoles.AdminOfficer, SmartSchoolRoles.Principal, SmartSchoolRoles.Examiner }.Any(user.IsInRole);
}
