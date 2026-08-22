namespace SmartSchool.Modules.Transport.Features.Driver;

/// <summary>Request model aligned to transport.driver.</summary>
public sealed record DriverRequest(Guid TenantId, Guid? EmployeeId, string DriverNumber,
    string FullName, string CnicNumber, string DrivingLicenseNumber, string? Phone,
    string? AlternatePhone, DateOnly? DateOfBirth, string? DrivingLicenseCategory,
    DateOnly? DrivingLicenseIssuedOn, DateOnly? DrivingLicenseExpiresOn, byte[]? Picture,
    string? PictureContentType, string? PictureFileName, string? EmergencyContactName,
    string? EmergencyContactPhone, string? Address, DateOnly? HireDate, string Status);

/// <summary>Detailed driver response; used by GetById.</summary>
public sealed record DriverResponse(Guid TenantId, Guid Id, Guid? EmployeeId, string DriverNumber,
    string FullName, string CnicNumber, string DrivingLicenseNumber, string? Phone,
    string? AlternatePhone, DateOnly? DateOfBirth, string? DrivingLicenseCategory,
    DateOnly? DrivingLicenseIssuedOn, DateOnly? DrivingLicenseExpiresOn, byte[]? Picture,
    string? PictureContentType, string? PictureFileName, string? EmergencyContactName,
    string? EmergencyContactPhone, string? Address, DateOnly? HireDate, string Status);

/// <summary>Lightweight driver page response without picture bytes.</summary>
public sealed record DriverPageResponse(Guid TenantId, Guid Id, string DriverNumber,
    string FullName, string CnicNumber, string DrivingLicenseNumber, string? Phone,
    DateOnly? DrivingLicenseExpiresOn, string Status);
