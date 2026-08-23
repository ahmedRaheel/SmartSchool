using SmartSchool.SharedKernel.Constants;
using SmartSchool.Application.Identity;

namespace SmartSchool.Infrastructure.Identity;

public sealed class TenantScope(ICurrentUser currentUser) : ITenantScope
{
    public bool IsSuperAdmin => currentUser.IsInRole(SmartSchoolRoles.SuperAdmin);
    public Guid UserId => currentUser.UserId;
    public Guid? TenantId => IsSuperAdmin ? null : currentUser.TenantId;

    public Guid? Resolve(Guid? requestedTenantId = null)
    {
        // SuperAdmin is platform-scoped. A tenant is a view/filter, never a security boundary.
        if (IsSuperAdmin) return null;
        var tokenTenant = currentUser.TenantId;
        if (requestedTenantId.HasValue && requestedTenantId.Value != tokenTenant)
            throw new UnauthorizedAccessException("The requested tenant is outside the authenticated tenant scope.");
        return tokenTenant;
    }
}
