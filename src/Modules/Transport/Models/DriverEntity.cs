using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Models;

/// <summary>
/// Represents an authorized school transport driver.
/// </summary>
public sealed class DriverEntity : Entity
{
    /// <summary>Gets the entity-specific identifier.</summary>
    public Guid DriverId { get; private set; } = Guid.NewGuid();

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

    /// <summary>Gets the persisted employee id value.</summary>
    public Guid? EmployeeId { get; private set; }

    /// <summary>Gets the persisted driver number value.</summary>
    public string DriverNumber { get; private set; } = string.Empty;

    /// <summary>Gets the persisted full name value.</summary>
    public string FullName { get; private set; } = string.Empty;

    /// <summary>Gets the persisted cnic number value.</summary>
    public string CnicNumber { get; private set; } = string.Empty;

    /// <summary>Gets the persisted phone value.</summary>
    public string? Phone { get; private set; }

    /// <summary>Gets the persisted alternate phone value.</summary>
    public string? AlternatePhone { get; private set; }

    /// <summary>Gets the persisted driving license issued on value.</summary>
    public DateOnly? DrivingLicenseIssuedOn { get; private set; }

    /// <summary>Gets the persisted driving license expires on value.</summary>
    public DateOnly? DrivingLicenseExpiresOn { get; private set; }

    /// <summary>Gets the persisted picture value.</summary>
    public byte[]? Picture { get; private set; }

    /// <summary>Gets the persisted picture content type value.</summary>
    public string? PictureContentType { get; private set; }

    /// <summary>Gets the persisted picture file name value.</summary>
    public string? PictureFileName { get; private set; }

    /// <summary>Gets the persisted address value.</summary>
    public string? Address { get; private set; }

    /// <summary>Gets the persisted hire date value.</summary>
    public DateOnly? HireDate { get; private set; }

    /// <summary>Gets the persisted status value.</summary>
    public string Status { get; private set; } = string.Empty;
    public static DriverEntity Register(Guid tenantId, Guid employeeId, string number, string firstName, string? lastName,
        string cnic, string? phone, DateOnly dateOfBirth, DateOnly hireDate, string licenseNumber, string licenseCategory, DateOnly licenseExpiry)
    {
        return new DriverEntity
        {
            TenantId = tenantId, EmployeeId = employeeId, DriverNumber = number, EmployeeNumber = number,
            FirstName = firstName, LastName = lastName ?? string.Empty, FullName = $"{firstName} {lastName}".Trim(),
            Cnic = cnic, CnicNumber = cnic, Phone = phone, MobileNumber = phone ?? string.Empty,
            DateOfBirth = dateOfBirth, HireDate = hireDate, JoiningDate = hireDate,
            DrivingLicenseNumber = licenseNumber.Trim(), DrivingLicenseCategory = licenseCategory.Trim(),
            DrivingLicenseExpiresOn = licenseExpiry, LicenseExpiryDate = licenseExpiry, EmploymentStatusCode = "ACTIVE", Status = "ACTIVE"
        };
    }
}
