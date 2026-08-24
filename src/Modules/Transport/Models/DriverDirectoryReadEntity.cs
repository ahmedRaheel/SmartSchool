using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Models;

/// <summary>
/// Denormalized read table optimized for directory/list queries.
/// It is rebuilt from normalized transactional tables and is not the system of record.
/// </summary>
public sealed class DriverDirectoryReadEntity : Entity
{
	/// <summary>Gets the persisted entity identifier.</summary>
	public Guid Id
	{
		get => Id;
		private set => Id = value;
	}

	private DriverDirectoryReadEntity()
	{
	}

	/// <summary>Gets the source aggregate identifier.</summary>
	public Guid DriverId { get; private set; }

	/// <summary>Gets the materialized EmployeeNumber value.</summary>
	public string EmployeeNumber { get; private set; } = string.Empty;
	/// <summary>Gets the materialized DriverName value.</summary>
	public string DriverName { get; private set; } = string.Empty;
	/// <summary>Gets the materialized MobileNumber value.</summary>
	public string? MobileNumber { get; private set; }
	/// <summary>Gets the materialized LicenseNumber value.</summary>
	public string LicenseNumber { get; private set; } = string.Empty;
	/// <summary>Gets the materialized LicenseExpiryDate value.</summary>
	public DateOnly? LicenseExpiryDate { get; private set; }
	/// <summary>Gets the materialized VehicleRegistrationNumber value.</summary>
	public string? VehicleRegistrationNumber { get; private set; }
	/// <summary>Gets the materialized RouteName value.</summary>
	public string? RouteName { get; private set; }
	/// <summary>Gets the materialized DocumentCount value.</summary>
	public int DocumentCount { get; private set; }
	/// <summary>Gets the materialized VerifiedDocumentCount value.</summary>
	public int VerifiedDocumentCount { get; private set; }

	/// <summary>Creates or replaces a materialized read row.</summary>
	public static DriverDirectoryReadEntity Create(
		Guid tenantId,
		Guid driverId,
		string employeeNumber,
		string driverName,
		string? mobileNumber,
		string licenseNumber,
		DateOnly? licenseExpiryDate,
		string? vehicleRegistrationNumber,
		string? routeName,
		int documentCount,
		int verifiedDocumentCount)
	{
		return new DriverDirectoryReadEntity
		{
			TenantId = tenantId,
			DriverId = driverId,
			EmployeeNumber = employeeNumber,
			DriverName = driverName,
			MobileNumber = mobileNumber,
			LicenseNumber = licenseNumber,
			LicenseExpiryDate = licenseExpiryDate,
			VehicleRegistrationNumber = vehicleRegistrationNumber,
			RouteName = routeName,
			DocumentCount = documentCount,
			VerifiedDocumentCount = verifiedDocumentCount,
		};
	}
}
