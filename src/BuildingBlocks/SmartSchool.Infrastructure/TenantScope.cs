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
        if (IsSuperAdmin)
            return requestedTenantId;

        return currentUser.TenantId
            ?? throw new UnauthorizedAccessException("The authenticated user does not contain a tenant scope.");
    }
}
