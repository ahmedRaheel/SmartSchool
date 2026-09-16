using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Learning.Authorization;

public static class LearningPermissions
{
    public static bool CanManageAll(ICurrentUser user) => user.IsSuperAdmin ||
        new[] { SmartSchoolRoles.Tenant, SmartSchoolRoles.TenantAdmin, SmartSchoolRoles.Owner,
            SmartSchoolRoles.Principal, SmartSchoolRoles.Admin, SmartSchoolRoles.AdminOfficer }
            .Any(user.IsInRole);

    public static bool CanManage(ICurrentUser user, Guid teacherEmployeeId) =>
        CanManageAll(user) || (user.IsInRole(SmartSchoolRoles.Teacher) &&
            (user.EmployeeId ?? user.TeacherId) == teacherEmployeeId);
}
