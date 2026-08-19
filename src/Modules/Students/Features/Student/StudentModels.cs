namespace SmartSchool.Modules.Students.Features.Student;

/// <summary>Request model aligned to student.student.</summary>
public sealed record StudentRequest(
    Guid TenantId, Guid? UserId, string StudentNumber, string FirstName, string? LastName,
    DateOnly? DateOfBirth, string? Gender, byte[]? Photo, string? PhotoContentType,
    string? PhotoFileName, DateOnly? AdmissionDate, string Status);

/// <summary>Detailed student response. Binary image is returned by GetById only.</summary>
public sealed record StudentResponse(
    Guid TenantId, Guid Id, Guid? UserId, string StudentNumber, string FirstName, string? LastName,
    DateOnly? DateOfBirth, string? Gender, byte[]? Photo, string? PhotoContentType,
    string? PhotoFileName, DateOnly? AdmissionDate, string Status);

/// <summary>Lightweight student page response without image bytes.</summary>
public sealed record StudentPageResponse(
    Guid TenantId, Guid Id, string StudentNumber, string FirstName, string? LastName,
    DateOnly? DateOfBirth, string? Gender, DateOnly? AdmissionDate, string Status);
