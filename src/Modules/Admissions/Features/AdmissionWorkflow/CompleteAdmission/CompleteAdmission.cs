using Dapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    TimeProvider timeProvider) : ICompleteAdmission
{
    private sealed record SectionAvailability(
        int? Capacity,
        bool IsActive,
        long ActiveEnrollmentCount);

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
        await using var transaction = await db.Database.BeginTransactionAsync(cancellationToken);

        const string sectionAvailabilitySql =
            """
            SELECT
                section.capacity AS "Capacity",
                section.is_active AS "IsActive",
                (
                    SELECT COUNT(*)
                    FROM student.student_enrollment enrollment
                    WHERE enrollment.tenant_id = section.tenant_id
                      AND enrollment.class_section_id = section.class_section_id
                      AND enrollment.is_active = TRUE
                      AND enrollment.status = 'ACTIVE'
                ) AS "ActiveEnrollmentCount"
            FROM academic.class_section section
            WHERE section.tenant_id = @TenantId
              AND section.class_section_id = @ClassSectionId
            FOR UPDATE;
            """;

        var connection = db.Database.GetDbConnection();
        var placement = await connection.QuerySingleOrDefaultAsync<SectionAvailability>(
            new CommandDefinition(
                sectionAvailabilitySql,
                new
                {
                    TenantId = tenantId,
                    application.ClassSectionId
                },
                transaction.GetDbTransaction(),
                cancellationToken: cancellationToken));

        if (placement is null || !placement.IsActive)
        {
            throw new ValidationException("The selected class is no longer available.");
        }

        if (placement.Capacity.HasValue &&
            placement.ActiveEnrollmentCount >= placement.Capacity.Value)
        {
            throw new ValidationException("The selected class has reached its capacity.");
        }
        var admissionDate = DateOnly.FromDateTime(timeProvider.GetUtcNow().UtcDateTime);
        var relationship = string.IsNullOrWhiteSpace(application.Relationship)
            ? "GUARDIAN"
            : application.Relationship.Trim();

        var admission = await db.AdmissionApplications
            .FromSqlInterpolated($"SELECT * FROM admission.student_application WHERE tenant_id = {tenantId} AND application_id = {application.Id} FOR UPDATE")
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

        if (application.AcademicYearId.HasValue && application.ClassSectionId.HasValue)
        {
            var enrollment = CompleteAdmissionEnrollment.Create(
                Guid.NewGuid(),
                tenantId,
                studentId,
                "ENR-" + studentNumber,
                application.AcademicYearId.Value,
                application.ClassSectionId.Value,
                admissionDate);

            await db.CompleteAdmissionEnrollments.AddAsync(enrollment, cancellationToken);
        }

        var documents = await db.CompleteAdmissionDocuments.Where(d => d.TenantId == tenantId && d.OwnerId == application.Id && d.OwnerType == "AdmissionDocument").ToListAsync(cancellationToken);
        foreach (var document in documents)
            document.AssignStudent(studentId);
        admission.RecordReview(entranceTestMarks, interviewPassed);
        admission.Accept(studentId, notes, timeProvider.GetUtcNow().UtcDateTime);

        await db.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}

public sealed class CompleteAdmissionStudent
{
    public Guid StudentId
    {
        get; private set;
    }
    public Guid TenantId
    {
        get; private set;
    }
    public Guid UserId
    {
        get; private set;
    }
    public Guid SchoolId
    {
        get; private set;
    }
    public Guid BranchId
    {
        get; private set;
    }
    public string StudentNumber { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string? LastName
    {
        get; private set;
    }
    public DateOnly? DateOfBirth
    {
        get; private set;
    }
    public string? Gender
    {
        get; private set;
    }
    public DateOnly AdmissionDate
    {
        get; private set;
    }
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
    public Guid GuardianId
    {
        get; private set;
    }
    public Guid TenantId
    {
        get; private set;
    }
    public Guid UserId
    {
        get; private set;
    }
    public string FullName { get; private set; } = string.Empty;
    public string? CnicNumber
    {
        get; private set;
    }
    public string? Email
    {
        get; private set;
    }
    public string? Phone
    {
        get; private set;
    }

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
    public Guid StudentGuardianId
    {
        get; private set;
    }
    public Guid TenantId
    {
        get; private set;
    }
    public Guid StudentId
    {
        get; private set;
    }
    public Guid GuardianId
    {
        get; private set;
    }
    public string Relationship { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; } = true;
    public bool CanViewAcademics { get; private set; } = true;
    public bool CanViewFinance { get; private set; } = true;
    public bool CanPickup
    {
        get; private set;
    }

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
    public Guid StudentEnrollmentId
    {
        get; private set;
    }
    public bool IsActive { get; private set; } = true;
    public Guid TenantId
    {
        get; private set;
    }
    public Guid StudentId
    {
        get; private set;
    }
    public string EnrollmentNumber { get; private set; } = string.Empty;
    public Guid AcademicYearId
    {
        get; private set;
    }
    public Guid ClassSectionId
    {
        get; private set;
    }
    public DateOnly EnrollmentDate
    {
        get; private set;
    }
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

public sealed class CompleteAdmissionDocument
{
    public Guid DocumentId
    {
        get; private set;
    }
    public Guid TenantId
    {
        get; private set;
    }
    public Guid OwnerId
    {
        get; private set;
    }
    public string OwnerType { get; private set; } = string.Empty;
    public void AssignStudent(Guid studentId)
    {
        OwnerId = studentId;
        OwnerType = "StudentDocument";
    }
}
