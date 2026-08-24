using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Represents a parent or guardian belonging to a SmartSchool tenant.
/// </summary>
public sealed class GuardianEntity : Entity
{
	/// <summary>Gets the entity-specific identifier.</summary>
	public Guid GuardianId { get; private set; } = Guid.NewGuid();

	private GuardianEntity()
	{
	}

	/// <summary>Gets the optional authenticated user identifier.</summary>
	public Guid? UserId { get; private set; }

	/// <summary>Gets the guardian full name.</summary>
	public string FullName { get; private set; } = string.Empty;

	/// <summary>Gets the guardian CNIC number.</summary>
	public string? CnicNumber { get; private set; }

	/// <summary>Gets the guardian email address.</summary>
	public string? Email { get; private set; }

	/// <summary>Gets the guardian phone number.</summary>
	public string? Phone { get; private set; }

	/// <summary>Creates a guardian.</summary>
	public static GuardianEntity Create(
		Guid tenantId,
		Guid? userId,
		string fullName,
		string? cnicNumber,
		string? email,
		string? phone)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

		return new GuardianEntity
		{
			TenantId = tenantId,
			UserId = userId,
			FullName = fullName.Trim(),
			CnicNumber = cnicNumber?.Trim(),
			Email = email?.Trim(),
			Phone = phone?.Trim()
		};
	}

	/// <summary>Updates guardian details.</summary>
	public void UpdateDetails(
		string fullName,
		string? cnicNumber,
		string? email,
		string? phone)
	{
		ArgumentException.ThrowIfNullOrWhiteSpace(fullName);

		FullName = fullName.Trim();
		CnicNumber = cnicNumber?.Trim();
		Email = email?.Trim();
		Phone = phone?.Trim();
		MarkAsUpdated();
	}
}
