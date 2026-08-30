namespace SmartSchool.Modules.Students.Features.Student;

public interface IStudentOnboardingQuery
{
    Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken);
    Task<bool> HasGuardianAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken);
    Task<IReadOnlyList<string>> GetMissingRequiredDocumentsAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken);
    Task<AdmissionPlacementReadModel?> GetPendingPlacementAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken);
    Task<string?> GetCampusCodeAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken);
    Task<bool> StudentAndGuardianBelongToTenantAsync(Guid tenantId, Guid studentId, Guid guardianId, CancellationToken cancellationToken);
}

public sealed record AdmissionPlacementReadModel(Guid AcademicYearId, Guid ClassSectionId, Guid ClassId);
