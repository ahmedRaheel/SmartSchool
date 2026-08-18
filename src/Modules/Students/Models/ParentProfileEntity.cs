using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Models;

/// <summary>
/// Stores guardian identity and contact information. CNIC is retained for adult guardians where legally required.
/// </summary>
public sealed class ParentProfileEntity : Entity
{
	public string FirstName { get; private set; } = string.Empty;
	public string LastName { get; private set; } = string.Empty;
	public string Cnic { get; private set; } = string.Empty;
	public string RelationshipCode { get; private set; } = string.Empty;
	public string MobileNumber { get; private set; } = string.Empty;
	public string? AlternateMobileNumber { get; private set; }
	public string? EmailAddress { get; private set; }
	public string? Occupation { get; private set; }
	public string? EmployerName { get; private set; }
	public string? WorkAddress { get; private set; }
	public string? ResidentialAddress { get; private set; }
	public bool IsPrimaryGuardian { get; private set; }
	public bool IsEmergencyContact { get; private set; }
	public bool CanCollectStudent { get; private set; }

	private ParentProfileEntity() { }
}
