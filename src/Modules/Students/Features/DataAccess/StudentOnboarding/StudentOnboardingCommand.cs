using SmartSchool.Modules.Students.Persistence;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Features.DataAccess.StudentOnboarding;

public sealed class StudentOnboardingCommand(IStudentsDbContext dbContext) : IStudentOnboardingCommand
{
    public async Task AddPlacementAsync(AdmissionPlacementEntity placement, CancellationToken cancellationToken)
    {
        await dbContext.AdmissionPlacements.AddAsync(placement, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddEnrollmentAndApprovePlacementAsync(EnrollmentEntity enrollment, Guid tenantId, Guid studentId, Guid academicYearId, CancellationToken cancellationToken)
    {
        var placement = await dbContext.AdmissionPlacements
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.StudentId == studentId && x.AcademicYearId == academicYearId && x.Status == LifecycleStatuses.Pending, cancellationToken);
        await dbContext.Enrollments.AddAsync(enrollment, cancellationToken);
        placement?.Approve();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddGuardianLinkAsync(StudentGuardianEntity link, CancellationToken cancellationToken)
    {
        await dbContext.StudentGuardians.AddAsync(link, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
