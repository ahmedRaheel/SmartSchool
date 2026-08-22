using SmartSchool.Application.Documents;

namespace SmartSchool.Modules.Students.Features.ParentProfile;

/// <summary>Lightweight parent/guardian row used by paged/list APIs.</summary>
public sealed record ParentSummaryResponse(
    Guid TenantId,
    Guid Id,
    string FirstName,
    string LastName,
    string RelationshipCode,
    string MobileNumber,
    string? EmailAddress,
    bool IsPrimaryGuardian);

/// <summary>Detailed parent/guardian response used by get-by-id.</summary>
public sealed record ParentDetailResponse(
    Guid TenantId,
    Guid Id,
    string FirstName,
    string LastName,
    string Cnic,
    string RelationshipCode,
    string MobileNumber,
    string? AlternateMobileNumber,
    string? EmailAddress,
    string? Occupation,
    string? EmployerName,
    string? WorkAddress,
    string? ResidentialAddress,
    bool IsPrimaryGuardian,
    bool IsEmergencyContact,
    bool CanCollectStudent,
    IReadOnlyCollection<DocumentResponse> Documents);
