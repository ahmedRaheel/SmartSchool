using SmartSchool.Application.Documents;

namespace SmartSchool.Modules.Students.Features.StudentProfile;

/// <summary>Lightweight student row used by paged/list APIs.</summary>
public sealed record StudentSummaryResponse(
    Guid TenantId,
    Guid Id,
    Guid StudentId,
    string AdmissionNumber,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string GenderCode,
    Guid? CurrentProgramId,
    Guid? CurrentClassId,
    Guid? CurrentSectionId);

/// <summary>Detailed student response used by get-by-id.</summary>
public sealed record StudentDetailResponse(
    Guid TenantId,
    Guid Id,
    Guid StudentId,
    string AdmissionNumber,
    string FirstName,
    string? MiddleName,
    string LastName,
    DateOnly DateOfBirth,
    string GenderCode,
    string? BFormNumber,
    string? PassportNumber,
    string? BloodGroupCode,
    string? PrimaryLanguageCode,
    string? MobileNumber,
    string? EmailAddress,
    string? AddressLine1,
    string? AddressLine2,
    string? City,
    string? Province,
    string? PostalCode,
    string? CountryCode,
    string? EmergencyContactName,
    string? EmergencyContactPhone,
    string? MedicalNotes,
    string? Allergies,
    DateOnly AdmissionDate,
    Guid? CurrentProgramId,
    Guid? CurrentClassId,
    Guid? CurrentSectionId,
    IReadOnlyCollection<DocumentResponse> Documents);
