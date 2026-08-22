using SmartSchool.Application.Documents;

namespace SmartSchool.Modules.HR.Features.TeacherProfile;

/// <summary>Lightweight teacher row used by paged/list APIs.</summary>
public sealed record TeacherSummaryResponse(
    Guid TenantId,
    Guid Id,
    Guid EmployeeId,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string? MobileNumber,
    string? Qualification,
    string? Specialization,
    Guid? DepartmentId,
    Guid? JobId,
    Guid? JobGradeId,
    string EmploymentStatusCode);

/// <summary>Detailed teacher response used by get-by-id.</summary>
public sealed record TeacherDetailResponse(
    Guid TenantId,
    Guid Id,
    Guid EmployeeId,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Cnic,
    DateOnly DateOfBirth,
    string GenderCode,
    string MobileNumber,
    string? EmailAddress,
    string? Qualification,
    string? Specialization,
    int? TeachingExperienceYears,
    DateOnly JoiningDate,
    Guid? DepartmentId,
    Guid? JobId,
    Guid? JobGradeId,
    string EmploymentStatusCode,
    IReadOnlyCollection<DocumentResponse> Documents);
