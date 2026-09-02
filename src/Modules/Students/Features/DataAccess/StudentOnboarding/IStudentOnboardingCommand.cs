using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.DataAccess.StudentOnboarding;

public interface IStudentOnboardingCommand
{
    Task AddPlacementAsync(AdmissionPlacementEntity placement, CancellationToken cancellationToken);
    Task AddEnrollmentAndApprovePlacementAsync(EnrollmentEntity enrollment, Guid tenantId, Guid studentId, Guid academicYearId, CancellationToken cancellationToken);
    Task AddGuardianLinkAsync(StudentGuardianEntity link, CancellationToken cancellationToken);
}
