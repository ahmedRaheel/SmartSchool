using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Modules.Admissions.Persistence;

namespace SmartSchool.Modules.Admissions.Features;

public interface ICompleteAdmission
{
    Task ExecuteAsync(
        Guid tenantId,
        AdmissionApplicationDetails application,
        Guid studentId,
        Guid studentUserId,
        Guid guardianId,
        Guid guardianUserId,
        string studentNumber,
        string? notes,
        decimal? entranceTestMarks,
        bool? interviewPassed,
        CancellationToken cancellationToken);
}

public sealed class CompleteAdmissionCommand(
    IAdmissionsDbContext db,
    IAdmissionsExternalPort externalPort,
    TimeProvider timeProvider) : ICompleteAdmission
{
    public async Task ExecuteAsync(
        Guid tenantId,
        AdmissionApplicationDetails application,
        Guid studentId,
        Guid studentUserId,
        Guid guardianId,
        Guid guardianUserId,
        string studentNumber,
        string? notes,
        decimal? entranceTestMarks,
        bool? interviewPassed,
        CancellationToken cancellationToken)
    {
        if (!application.ClassSectionId.HasValue)
        {
            throw new ValidationException("A class section is required before admission can be completed.");
        }

        var placement = await externalPort.GetClassSectionAvailabilityAsync(
            tenantId,
            application.ClassSectionId.Value,
            cancellationToken);

        if (placement is null || !placement.IsActive)
        {
            throw new ValidationException("The selected class is no longer available.");
        }

        if (placement.Capacity.HasValue)
        {
            var enrollmentCount = await externalPort.GetActiveEnrollmentCountAsync(
                tenantId,
                application.ClassSectionId.Value,
                cancellationToken);

            if (enrollmentCount >= placement.Capacity.Value)
            {
                throw new ValidationException("The selected class has reached its capacity.");
            }
        }

        var admission = await db.AdmissionApplications
            .SingleOrDefaultAsync(
                entity => entity.ApplicationId == application.Id &&
                          entity.TenantId == tenantId,
                cancellationToken);

        if (admission is null)
        {
            throw new InvalidOperationException("Admission application was not found.");
        }

        if (admission.StudentId.HasValue)
        {
            throw new InvalidOperationException("This application has already been admitted.");
        }

        var admissionDate = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        var relationship = string.IsNullOrWhiteSpace(application.Relationship)
            ? "GUARDIAN"
            : application.Relationship.Trim().ToUpperInvariant();

        await externalPort.ProvisionAdmissionAsync(
            new AdmissionProvisioningRequest(
                tenantId,
                application.Id,
                studentId,
                studentUserId,
                guardianId,
                guardianUserId,
                application.SchoolId,
                application.BranchId,
                application.AcademicYearId,
                application.ClassSectionId,
                studentNumber,
                application.FirstName,
                application.LastName,
                application.DateOfBirth,
                application.Gender,
                application.GuardianName,
                application.GuardianCnic,
                application.GuardianEmail,
                application.GuardianPhone,
                relationship,
                admissionDate),
            cancellationToken);

        admission.RecordReview(entranceTestMarks, interviewPassed);
        admission.Accept(studentId, notes, timeProvider.GetUtcNow().UtcDateTime);

        await db.SaveChangesAsync(cancellationToken);
    }
}
