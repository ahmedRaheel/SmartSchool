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
        CancellationToken cancellationToken);
}

public sealed class CompleteAdmissionCommand(
    IAdmissionsDbContext db,
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
        CancellationToken cancellationToken)
    {
        var admissionDate = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        var relationship = string.IsNullOrWhiteSpace(application.Relationship)
            ? "GUARDIAN"
            : application.Relationship.Trim();

        var admission = await db.CompleteAdmissionApplications
            .SingleOrDefaultAsync(
                entity => entity.ApplicationId == application.Id &&
                          entity.TenantId == tenantId,
                cancellationToken);

        if (admission is null)
        {
            throw new InvalidOperationException("Admission application was not found.");
        }

        var student = CompleteAdmissionStudent.Create(
            studentId,
            tenantId,
            studentUserId,
            application.SchoolId,
            application.BranchId,
            studentNumber,
            application.FirstName,
            application.LastName,
            application.DateOfBirth,
            application.Gender,
            admissionDate);

        var guardian = CompleteAdmissionGuardian.Create(
            guardianId,
            tenantId,
            guardianUserId,
            application.GuardianName,
            application.GuardianCnic,
            application.GuardianEmail,
            application.GuardianPhone);

        var studentGuardian = CompleteAdmissionStudentGuardian.Create(
            Guid.NewGuid(),
            tenantId,
            studentId,
            guardianId,
            relationship);

        await db.CompleteAdmissionStudents.AddAsync(student, cancellationToken);
        await db.CompleteAdmissionGuardians.AddAsync(guardian, cancellationToken);
        await db.CompleteAdmissionStudentGuardians.AddAsync(studentGuardian, cancellationToken);

        if (application.AcademicYearId.HasValue && application.SectionId.HasValue)
        {
            var enrollment = CompleteAdmissionEnrollment.Create(
                Guid.NewGuid(),
                tenantId,
                studentId,
                studentNumber,
                application.AcademicYearId.Value,
                application.SectionId.Value,
                admissionDate);

            await db.CompleteAdmissionEnrollments.AddAsync(enrollment, cancellationToken);
        }

        admission.Accept(studentId, notes, timeProvider.GetUtcNow().UtcDateTime);

        await db.SaveChangesAsync(cancellationToken);
    }
}

public sealed class CompleteAdmissionStudent
{
    public Guid StudentId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }
    public Guid SchoolId { get; private set; }
    public Guid BranchId { get; private set; }
    public string StudentNumber { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string? LastName { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public string? Gender { get; private set; }
    public DateOnly AdmissionDate { get; private set; }
    public string Status { get; private set; } = "ACTIVE";

    private CompleteAdmissionStudent()
    {
    }

    public static CompleteAdmissionStudent Create(
        Guid studentId,
        Guid tenantId,
        Guid userId,
        Guid schoolId,
        Guid branchId,
        string studentNumber,
        string firstName,
        string? lastName,
        DateOnly? dateOfBirth,
        string? gender,
        DateOnly admissionDate)
    {
        return new CompleteAdmissionStudent
        {
            StudentId = studentId,
            TenantId = tenantId,
            UserId = userId,
            SchoolId = schoolId,
            BranchId = branchId,
            StudentNumber = studentNumber.Trim(),
            FirstName = firstName.Trim(),
            LastName = lastName?.Trim(),
            DateOfBirth = dateOfBirth,
            Gender = gender?.Trim(),
            AdmissionDate = admissionDate
        };
    }
}

public sealed class CompleteAdmissionGuardian
{
    public Guid GuardianId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid UserId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string? CnicNumber { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }

    private CompleteAdmissionGuardian()
    {
    }

    public static CompleteAdmissionGuardian Create(
        Guid guardianId,
        Guid tenantId,
        Guid userId,
        string fullName,
        string? cnicNumber,
        string? email,
        string? phone)
    {
        return new CompleteAdmissionGuardian
        {
            GuardianId = guardianId,
            TenantId = tenantId,
            UserId = userId,
            FullName = fullName.Trim(),
            CnicNumber = cnicNumber?.Trim(),
            Email = email?.Trim(),
            Phone = phone?.Trim()
        };
    }
}

public sealed class CompleteAdmissionStudentGuardian
{
    public Guid StudentGuardianId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid StudentId { get; private set; }
    public Guid GuardianId { get; private set; }
    public string Relationship { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; } = true;
    public bool CanViewAcademics { get; private set; } = true;
    public bool CanViewFinance { get; private set; } = true;
    public bool CanPickup { get; private set; }

    private CompleteAdmissionStudentGuardian()
    {
    }

    public static CompleteAdmissionStudentGuardian Create(
        Guid studentGuardianId,
        Guid tenantId,
        Guid studentId,
        Guid guardianId,
        string relationship)
    {
        return new CompleteAdmissionStudentGuardian
        {
            StudentGuardianId = studentGuardianId,
            TenantId = tenantId,
            StudentId = studentId,
            GuardianId = guardianId,
            Relationship = relationship.Trim().ToUpperInvariant()
        };
    }
}

public sealed class CompleteAdmissionEnrollment
{
    public Guid StudentEnrollmentId { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid StudentId { get; private set; }
    public string EnrollmentNumber { get; private set; } = string.Empty;
    public Guid AcademicYearId { get; private set; }
    public Guid ClassSectionId { get; private set; }
    public DateOnly EnrollmentDate { get; private set; }
    public string Status { get; private set; } = "ACTIVE";

    private CompleteAdmissionEnrollment()
    {
    }

    public static CompleteAdmissionEnrollment Create(
        Guid studentEnrollmentId,
        Guid tenantId,
        Guid studentId,
        string enrollmentNumber,
        Guid academicYearId,
        Guid classSectionId,
        DateOnly enrollmentDate)
    {
        return new CompleteAdmissionEnrollment
        {
            StudentEnrollmentId = studentEnrollmentId,
            TenantId = tenantId,
            StudentId = studentId,
            EnrollmentNumber = enrollmentNumber.Trim(),
            AcademicYearId = academicYearId,
            ClassSectionId = classSectionId,
            EnrollmentDate = enrollmentDate
        };
    }
}

public sealed class CompleteAdmissionApplication
{
    public Guid ApplicationId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public Guid? StudentId { get; private set; }
    public string? DecisionNotes { get; private set; }
    public DateTime? DecidedAt { get; private set; }

    private CompleteAdmissionApplication()
    {
    }

    public void Accept(Guid studentId, string? notes, DateTime decidedAt)
    {
        StudentId = studentId;
        DecisionNotes = notes?.Trim();
        DecidedAt = decidedAt;
        Status = AdmissionApplicationStatus.AdmissionAccepted.ToDatabaseValue();
    }

    public void ChangeStatus(
        AdmissionApplicationStatus status,
        string? notes,
        DateTime decidedAt)
    {
        Status = status.ToDatabaseValue();
        DecisionNotes = notes?.Trim();
        DecidedAt = status == AdmissionApplicationStatus.SubmittedApplication
            ? null
            : decidedAt;
    }
}
