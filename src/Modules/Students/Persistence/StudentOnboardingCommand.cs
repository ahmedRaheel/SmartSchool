using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Students.Persistence;

public sealed class StudentOnboardingCommand(IApplicationDbContext dbContext) : IStudentOnboardingCommand
{
    public async Task AddPlacementAsync(AdmissionPlacementEntity placement, CancellationToken cancellationToken)
    {
        await dbContext.Set<AdmissionPlacementEntity>().AddAsync(placement, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddEnrollmentAndApprovePlacementAsync(EnrollmentEntity enrollment, Guid tenantId, Guid studentId, Guid academicYearId, CancellationToken cancellationToken)
    {
        var placement = await dbContext.Set<AdmissionPlacementEntity>()
            .FirstOrDefaultAsync(x => x.TenantId == tenantId && x.StudentId == studentId && x.AcademicYearId == academicYearId && x.Status == LifecycleStatuses.Pending, cancellationToken);
        await dbContext.Set<EnrollmentEntity>().AddAsync(enrollment, cancellationToken);
        placement?.Approve();
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddGuardianLinkAsync(StudentGuardianEntity link, CancellationToken cancellationToken)
    {
        await dbContext.Set<StudentGuardianEntity>().AddAsync(link, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
