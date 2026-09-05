using Microsoft.AspNetCore.Identity;

namespace SmartSchool.Modules.Identity.Infrastructure.Identity;

public sealed class SmartSchoolRole : IdentityRole<Guid>
{
    public Guid? TenantId { get; set; }
    public string? Description { get; set; }
    public bool IsSystemRole { get; set; }
}
