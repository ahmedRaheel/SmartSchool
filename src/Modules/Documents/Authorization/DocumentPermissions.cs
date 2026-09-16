using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Documents.Authorization;

/// <summary>Role checks shared by document endpoints; ownership is verified by each feature query.</summary>
public static class DocumentPermissions
{
    public static bool CanManage(ICurrentUser user)
    {
        return user.IsSuperAdmin || user.IsInRole(SmartSchoolRoles.Tenant)
            || user.IsInRole(SmartSchoolRoles.TenantAdmin) || user.IsInRole(SmartSchoolRoles.Owner)
            || user.IsInRole(SmartSchoolRoles.Principal) || user.IsInRole(SmartSchoolRoles.Admin)
            || user.IsInRole(SmartSchoolRoles.AdminOfficer) || user.IsInRole(SmartSchoolRoles.HrManager)
            || user.IsInRole(SmartSchoolRoles.HR);
    }
}
