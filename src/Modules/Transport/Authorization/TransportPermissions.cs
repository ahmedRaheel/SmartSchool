using SmartSchool.Application.Identity;
using SmartSchool.SharedKernel.Constants;
namespace SmartSchool.Modules.Transport.Authorization;
public static class TransportPermissions
{
    public static bool CanManage(ICurrentUser user) => user.IsSuperAdmin || new[] { SmartSchoolRoles.Tenant,
        SmartSchoolRoles.TenantAdmin, SmartSchoolRoles.Owner, SmartSchoolRoles.Principal, SmartSchoolRoles.Admin,
        SmartSchoolRoles.AdminOfficer }.Any(user.IsInRole);
}
