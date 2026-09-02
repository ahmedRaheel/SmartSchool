using Dapper;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Features;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features.AdmissionWorkflow;

internal sealed class AdmissionWorkflowCommand(IDbConnectionFactory connectionFactory)
    : IAdmissionWorkflowCommand
{
    public async Task<Guid> CreateApplicationAsync(
        Guid tenantId,
        CreateAdmissionApplication.Request request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO admission.student_application (
                application_id,
                tenant_id,
                school_id,
                branch_id,
                academic_year_id,
                class_id,
                section_id,
                first_name,
                last_name,
                date_of_birth,
                gender,
                email,
                phone,
                address,
                guardian_name,
                guardian_cnic,
                guardian_email,
                guardian_phone,
                relationship,
                previous_school,
                previous_marks,
                status)
            VALUES (
                @Id,
                @TenantId,
                @SchoolId,
                @BranchId,
                @AcademicYearId,
                @ClassId,
                @SectionId,
                @FirstName,
                @LastName,
                @DateOfBirth,
                @Gender,
                @Email,
                @Phone,
                @Address,
                @GuardianName,
                @GuardianCnic,
                @GuardianEmail,
                @GuardianPhone,
                @Relationship,
                @PreviousSchool,
                @PreviousMarks,
                'SUBMITTED_APPLICATION');
            """;

        var id = Guid.NewGuid();
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    Id = id,
                    TenantId = tenantId,
                    request.SchoolId,
                    request.BranchId,
                    request.AcademicYearId,
                    request.ClassId,
                    request.SectionId,
                    request.FirstName,
                    request.LastName,
                    request.DateOfBirth,
                    Gender = request.Gender?.ToString(),
                    request.Email,
                    request.Phone,
                    request.Address,
                    request.GuardianName,
                    request.GuardianCnic,
                    request.GuardianEmail,
                    request.GuardianPhone,
                    request.Relationship,
                    request.PreviousSchool,
                    request.PreviousMarks
                },
                cancellationToken: cancellationToken));

        return id;
    }

    public async Task<bool> ChangeStatusAsync(
        Guid tenantId,
        Guid applicationId,
        AdmissionApplicationStatus status,
        string? notes,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE admission.student_application
            SET status = @Status,
                decision_notes = @Notes,
                decided_at = CASE
                    WHEN @Status = 'SUBMITTED_APPLICATION' THEN NULL
                    ELSE NOW()
                END
            WHERE application_id = @ApplicationId
              AND tenant_id = @TenantId;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var affected = await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    ApplicationId = applicationId,
                    TenantId = tenantId,
                    Status = status.ToDatabaseValue(),
                    Notes = notes
                },
                cancellationToken: cancellationToken));

        return affected > 0;
    }

    public async Task CompleteAdmissionAsync(
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
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        try
        {
            await InsertStudentAsync(connection, transaction, tenantId, application, studentId, studentUserId, studentNumber, cancellationToken);
            await InsertGuardianAsync(connection, transaction, tenantId, application, guardianId, guardianUserId, cancellationToken);
            await LinkGuardianAsync(connection, transaction, studentId, guardianId, application.Relationship, cancellationToken);
            await CreateInitialEnrollmentAsync(connection, transaction, tenantId, application, studentId, studentNumber, cancellationToken);
            await MarkApplicationAcceptedAsync(connection, transaction, tenantId, application.Id, studentId, notes, cancellationToken);
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<Guid> CreateCriteriaAsync(
        Guid tenantId,
        CreateAdmissionCriteria.Request request,
        CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO admission.admission_criteria (
                admission_criteria_id,
                tenant_id,
                school_id,
                branch_id,
                academic_year_id,
                class_id,
                minimum_marks,
                entrance_test_minimum,
                minimum_age,
                maximum_age,
                interview_required,
                required_documents)
            VALUES (
                @Id,
                @TenantId,
                @SchoolId,
                @BranchId,
                @AcademicYearId,
                @ClassId,
                @MinimumMarks,
                @EntranceTestMinimum,
                @MinimumAge,
                @MaximumAge,
                @InterviewRequired,
                @RequiredDocuments);
            """;

        var id = Guid.NewGuid();
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(
            new CommandDefinition(
                sql,
                new
                {
                    Id = id,
                    TenantId = tenantId,
                    request.SchoolId,
                    request.BranchId,
                    request.AcademicYearId,
                    request.ClassId,
                    request.MinimumMarks,
                    request.EntranceTestMinimum,
                    request.MinimumAge,
                    request.MaximumAge,
                    request.InterviewRequired,
                    request.RequiredDocuments
                },
                cancellationToken: cancellationToken));

        return id;
    }

    private static async Task InsertStudentAsync(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction, Guid tenantId, AdmissionApplicationDetails application, Guid studentId, Guid userId, string studentNumber, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO student.student (
                student_id, tenant_id, user_id, school_id, branch_id, student_number,
                first_name, last_name, date_of_birth, gender, admission_date, status,
                is_active, created_at)
            VALUES (
                @StudentId, @TenantId, @UserId, @SchoolId, @BranchId, @StudentNumber,
                @FirstName, @LastName, @DateOfBirth, @Gender, CURRENT_DATE, 'ACTIVE',
                TRUE, NOW());
            """;

        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            StudentId = studentId,
            TenantId = tenantId,
            UserId = userId,
            application.SchoolId,
            application.BranchId,
            StudentNumber = studentNumber,
            application.FirstName,
            application.LastName,
            application.DateOfBirth,
            gender = application.Gender
        }, transaction, cancellationToken: cancellationToken));
    }

    private static async Task InsertGuardianAsync(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction, Guid tenantId, AdmissionApplicationDetails application, Guid guardianId, Guid userId, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO student.guardian (
                guardian_id, tenant_id, user_id, full_name, cnic_number, email, phone,
                is_active, created_at)
            VALUES (
                @GuardianId, @TenantId, @UserId, @Name, @Cnic, @Email, @Phone,
                TRUE, NOW());
            """;

        await connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            GuardianId = guardianId,
            TenantId = tenantId,
            UserId = userId,
            Name = application.GuardianName,
            Cnic = application.GuardianCnic,
            Email = application.GuardianEmail,
            Phone = application.GuardianPhone
        }, transaction, cancellationToken: cancellationToken));
    }

    private static Task LinkGuardianAsync(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction, Guid studentId, Guid guardianId, string? relationship, CancellationToken cancellationToken)
    {
        const string sql = """
            INSERT INTO student.student_guardian (
                student_id, guardian_id, relationship, is_primary,
                can_view_academics, can_view_finance, can_pickup)
            VALUES (
                @StudentId, @GuardianId, @Relationship, TRUE, TRUE, TRUE, TRUE);
            """;

        return connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            StudentId = studentId,
            GuardianId = guardianId,
            Relationship = relationship ?? SmartSchoolRoles.Parent
        }, transaction, cancellationToken: cancellationToken));
    }

    private static async Task CreateInitialEnrollmentAsync(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction, Guid tenantId, AdmissionApplicationDetails application, Guid studentId, string studentNumber, CancellationToken cancellationToken)
    {
        if (!application.AcademicYearId.HasValue || !application.ClassId.HasValue)
        {
            return;
        }

        const string findClassSectionSql = """
            SELECT class_section_id
            FROM academic.class_section
            WHERE tenant_id = @TenantId
              AND academic_year_id = @AcademicYearId
              AND class_id = @ClassId
              AND (@SectionId IS NULL OR section_id = @SectionId)
              AND is_active = TRUE
            LIMIT 1;
            """;

        var classSectionId = await connection.ExecuteScalarAsync<Guid?>(new CommandDefinition(
            findClassSectionSql,
            new
            {
                TenantId = tenantId,
                AcademicYearId = application.AcademicYearId.Value,
                ClassId = application.ClassId.Value,
                application.SectionId
            },
            transaction,
            cancellationToken: cancellationToken));

        if (!classSectionId.HasValue)
        {
            return;
        }

        const string insertEnrollmentSql = """
            INSERT INTO student.student_enrollment (
                student_enrollment_id, tenant_id, student_id, academic_year_id,
                class_section_id, enrollment_number, enrollment_date, status,
                is_active, created_at)
            VALUES (
                @EnrollmentId, @TenantId, @StudentId, @AcademicYearId,
                @ClassSectionId, @EnrollmentNumber, CURRENT_DATE, 'ACTIVE',
                TRUE, NOW());
            """;

        await connection.ExecuteAsync(new CommandDefinition(insertEnrollmentSql, new
        {
            EnrollmentId = Guid.NewGuid(),
            TenantId = tenantId,
            StudentId = studentId,
            AcademicYearId = application.AcademicYearId.Value,
            ClassSectionId = classSectionId.Value,
            EnrollmentNumber = $"{studentNumber}-001"
        }, transaction, cancellationToken: cancellationToken));
    }

    private static Task MarkApplicationAcceptedAsync(System.Data.IDbConnection connection, System.Data.IDbTransaction transaction, Guid tenantId, Guid applicationId, Guid studentId, string? notes, CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE admission.student_application
            SET status = 'ADMISSION_ACCEPTED',
                decision_notes = @Notes,
                decided_at = NOW(),
                student_id = @StudentId
            WHERE application_id = @ApplicationId
              AND tenant_id = @TenantId;
            """;

        return connection.ExecuteAsync(new CommandDefinition(sql, new
        {
            ApplicationId = applicationId,
            TenantId = tenantId,
            StudentId = studentId,
            Notes = notes
        }, transaction, cancellationToken: cancellationToken));
    }
}
