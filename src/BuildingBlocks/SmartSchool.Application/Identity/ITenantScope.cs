namespace SmartSchool.Application.Identity;

public interface ITenantScope
{
    bool IsSuperAdmin { get; }
    Guid? TenantId { get; }
    Guid UserId { get; }
    Guid? Resolve(Guid? requestedTenantId = null);
}
