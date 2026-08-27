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
        // Tenant users are always constrained to the tenant carried by their authenticated token.
        // A stale or manipulated tenantId supplied by the client is deliberately ignored instead
        // of becoming an authorization failure or, more importantly, widening data access.
        return currentUser.TenantId;
    }
}
