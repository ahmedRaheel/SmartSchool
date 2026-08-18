using SmartSchool.Application.Documents;

namespace SmartSchool.Modules.Transport.Features.Driver;

/// <summary>Lightweight driver row used by paged/list APIs.</summary>
public sealed record DriverSummaryResponse(
    Guid TenantId,
    Guid Id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string MobileNumber,
    DateOnly LicenseExpiryDate,
    string EmploymentStatusCode,
    Guid? AssignedVehicleId);

/// <summary>Detailed driver response used by get-by-id.</summary>
public sealed record DriverDetailResponse(
    Guid TenantId,
    Guid Id,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Cnic,
    DateOnly DateOfBirth,
    string MobileNumber,
    string DrivingLicenseNumber,
    string DrivingLicenseCategory,
    DateOnly LicenseExpiryDate,
    DateOnly JoiningDate,
    string EmploymentStatusCode,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    Guid? AssignedVehicleId,
    IReadOnlyCollection<DocumentResponse> Documents);
