using Microsoft.AspNetCore.Identity;

namespace SmartSchool.Modules.Identity.Persistence.Identity;

public sealed class SmartSchoolUser : IdentityUser<Guid>
{
	public Guid? TenantId { get; set; }
	public Guid? BusinessEntityId { get; set; }
	public string? AccountType { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string? DisplayName { get; set; }
	public bool IsActive { get; set; } = true;
	public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
	public DateTimeOffset? UpdatedAt { get; set; }
}
