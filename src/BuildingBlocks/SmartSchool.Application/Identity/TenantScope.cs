using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Application.Identity;

/// <summary>
/// Resolves tenant scope from the authenticated token for tenant users and from
/// the explicit request only for a platform SuperAdmin.
/// </summary>
public sealed class TenantScope(ICurrentUser currentUser) : ITenantScope
{
    public bool IsSuperAdmin => currentUser.IsInRole(SmartSchoolRoles.SuperAdmin) || currentUser.IsInRole(SmartSchoolRoles.SuperOwner);

    public Guid UserId => currentUser.UserId;

    public Guid? TenantId => IsSuperAdmin
        ? null
        : currentUser.TenantId;

    public Guid? Resolve(Guid? requestedTenantId = null)
    {
        if (IsSuperAdmin)
        {
            return requestedTenantId ?? currentUser.TenantId;
        }

        if (requestedTenantId.HasValue && requestedTenantId != Guid.Empty && requestedTenantId != currentUser.TenantId)
        {
            throw new UnauthorizedAccessException("The requested tenant is outside the authenticated tenant scope.");
        }

        return currentUser.TenantId
            ?? throw new UnauthorizedAccessException(
                "The authenticated access token does not contain a tenant scope.");
    }
}
