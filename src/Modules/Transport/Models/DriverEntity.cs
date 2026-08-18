using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Models;

/// <summary>
/// Represents an authorized school transport driver.
/// </summary>
public sealed class DriverEntity : Entity
{
	public string EmployeeNumber { get; private set; } = string.Empty;
	public string FirstName { get; private set; } = string.Empty;
	public string LastName { get; private set; } = string.Empty;
	public string Cnic { get; private set; } = string.Empty;
	public DateOnly DateOfBirth { get; private set; }
	public string MobileNumber { get; private set; } = string.Empty;
	public string DrivingLicenseNumber { get; private set; } = string.Empty;
	public string DrivingLicenseCategory { get; private set; } = string.Empty;
	public DateOnly LicenseExpiryDate { get; private set; }
	public DateOnly JoiningDate { get; private set; }
	public string EmploymentStatusCode { get; private set; } = string.Empty;
	public string? EmergencyContactName { get; private set; }
	public string? EmergencyContactPhone { get; private set; }
	public Guid? AssignedVehicleId { get; private set; }

	private DriverEntity() { }
}
