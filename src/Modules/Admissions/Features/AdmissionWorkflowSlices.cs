using Dapper;
using SmartSchool.Modules.Admissions.Features;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Features.AdmissionWorkflow;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;

namespace SmartSchool.Modules.Admissions.Features;

public enum AdmissionApplicationStatus
{
    SubmittedApplication,
    AdmissionAccepted,
    AdmissionRejected,
    WaitingList
}

public static class AdmissionApplicationStatusExtensions
{
    public static bool TryParseDatabaseValue(
        string? value,
        out AdmissionApplicationStatus status)
    {
        status = value?.Trim().ToUpperInvariant() switch
        {
            "SUBMITTED_APPLICATION" => AdmissionApplicationStatus.SubmittedApplication,
            "ADMISSION_ACCEPTED" => AdmissionApplicationStatus.AdmissionAccepted,
            "ADMISSION_REJECTED" => AdmissionApplicationStatus.AdmissionRejected,
            LifecycleStatuses.WaitingList => AdmissionApplicationStatus.WaitingList,
            _ => default
        };

        return value?.Trim().ToUpperInvariant() is
            "SUBMITTED_APPLICATION" or
            "ADMISSION_ACCEPTED" or
            "ADMISSION_REJECTED" or
            LifecycleStatuses.WaitingList;
    }

    public static string ToDatabaseValue(this AdmissionApplicationStatus status) => status switch
    {
        AdmissionApplicationStatus.SubmittedApplication => "SUBMITTED_APPLICATION",
        AdmissionApplicationStatus.AdmissionAccepted => "ADMISSION_ACCEPTED",
        AdmissionApplicationStatus.AdmissionRejected => "ADMISSION_REJECTED",
        AdmissionApplicationStatus.WaitingList => LifecycleStatuses.WaitingList,
        _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
    };
}

public sealed record AdmissionApplicationDto(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid? AcademicYearId,
    Guid? ClassId,
    Guid? SectionId,
    string FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? Gender,
    string? Email,
    string? Phone,
    string GuardianName,
    string? GuardianEmail,
    string? GuardianPhone,
    decimal? PreviousMarks,
    string Status,
    DateTimeOffset SubmittedAt,
    string? DecisionNotes,
    Guid? StudentId);

public sealed record AdmissionCriteriaDto(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid AcademicYearId,
    Guid ClassId,
    decimal MinimumMarks,
    decimal? EntranceTestMinimum,
    int? MinimumAge,
    int? MaximumAge,
    bool InterviewRequired,
    string? RequiredDocuments,
    string Status);

public sealed record AdmissionApplicationDetails(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid? AcademicYearId,
    Guid? ClassId,
    Guid? SectionId,
    string FirstName,
    string? LastName,
    DateOnly? DateOfBirth,
    string? Gender,
    string? Email,
    string GuardianName,
    string? GuardianCnic,
    string? GuardianEmail,
    string? GuardianPhone,
    string? Relationship,
    Guid? StudentId);

public static class GetAdmissionApplications
{
    public sealed record Request(Guid? TenantId)
        : IRequest<Result<IReadOnlyList<AdmissionApplicationDto>>>;

    public sealed class Handler(
        ITenantScope tenantScope,
        AdmissionWorkflowSlicesAdmissionWorkflowReadData query)
        : IRequestHandler<Request, Result<IReadOnlyList<AdmissionApplicationDto>>>
    {
        public async Task<Result<IReadOnlyList<AdmissionApplicationDto>>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<IReadOnlyList<AdmissionApplicationDto>>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var applications = await query.GetApplicationsAsync(
                tenantId.Value,
                cancellationToken);

            return Result<IReadOnlyList<AdmissionApplicationDto>>.Success(applications);
        }
    }
}

public static class CreateAdmissionApplication
{
    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid? AcademicYearId,
        Guid? ClassId,
        Guid? SectionId,
        string FirstName,
        string? LastName,
        DateOnly? DateOfBirth,
        string? Gender,
        string? Email,
        string? Phone,
        string? Address,
        string GuardianName,
        string? GuardianCnic,
        string? GuardianEmail,
        string? GuardianPhone,
        string? Relationship,
        string? PreviousSchool,
        decimal? PreviousMarks)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id, string Status);

    public sealed class Handler(
        ITenantScope tenantScope,
        AdmissionWorkflowSlicesAdmissionWorkflowReadData query,
        AdmissionWorkflowSlicesAdmissionWorkflowWriteData command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var branchIsValid = await query.BranchBelongsToSchoolAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                cancellationToken);

            if (!branchIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation("Selected branch does not belong to the selected school."));
            }

            if (string.IsNullOrWhiteSpace(request.Gender))
            {
                return Result<Response>.Failure(Error.Validation("Applicant gender is required."));
            }

            var branchGenderPolicy = await query.GetBranchGenderPolicyAsync(
                tenantId.Value,
                request.BranchId,
                cancellationToken);

            if (!GenderIsAllowed(branchGenderPolicy, request.Gender))
            {
                return Result<Response>.Failure(
                    Error.Validation("Applicant gender is not eligible for the selected branch."));
            }

            if (request.ClassId.HasValue)
            {
                var classIsEligible = await query.ClassIsEligibleForBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.ClassId.Value,
                    cancellationToken);

                if (!classIsEligible)
                {
                    return Result<Response>.Failure(
                        Error.Validation("The selected class is not available for this branch education level."));
                }
            }

            if (request.AcademicYearId.HasValue)
            {
                var academicYearIsValid = await query.AcademicYearBelongsToBranchAsync(
                    tenantId.Value,
                    request.BranchId,
                    request.AcademicYearId.Value,
                    cancellationToken);

                if (!academicYearIsValid)
                {
                    return Result<Response>.Failure(
                        Error.Validation("Academic year is not available for the selected branch."));
                }
            }

            var applicationId = await command.CreateApplicationAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(
                new Response(
                    applicationId,
                    AdmissionApplicationStatus.SubmittedApplication.ToDatabaseValue()));
        }

        private static bool GenderIsAllowed(string? branchPolicy, string applicantGender)
        {
            if (string.Equals(branchPolicy, "CO_EDUCATION", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (string.Equals(branchPolicy, "BOYS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "MALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "BOY", StringComparison.OrdinalIgnoreCase);
            }

            if (string.Equals(branchPolicy, "GIRLS_ONLY", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(applicantGender, "FEMALE", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(applicantGender, "GIRL", StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }
    }
}

public static class ChangeAdmissionStatus
{
    public sealed record Body(
        Guid? TenantId,
        string Status,
        string? Notes);

    public sealed record Request(
        Guid Id,
        Guid? TenantId,
        AdmissionApplicationStatus Status,
        string? Notes)
        : IRequest<Result<Response>>;

    public sealed record Response(
        Guid Id,
        string Status,
        string? StudentNumber = null);

    public sealed class Handler(
        ITenantScope tenantScope,
        AdmissionWorkflowSlicesAdmissionWorkflowReadData query,
        AdmissionWorkflowSlicesAdmissionWorkflowWriteData command,
        IIdentityAccountService accounts,
        IBusinessNumberGenerator numbers)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            if (request.Status != AdmissionApplicationStatus.AdmissionAccepted)
            {
                return await ChangeNonAdmissionStatusAsync(
                    tenantId.Value,
                    request,
                    cancellationToken);
            }

            return await AcceptAdmissionAsync(
                tenantId.Value,
                request,
                cancellationToken);
        }

        private async Task<Result<Response>> ChangeNonAdmissionStatusAsync(
            Guid tenantId,
            Request request,
            CancellationToken cancellationToken)
        {
            var changed = await command.ChangeStatusAsync(
                tenantId,
                request.Id,
                request.Status,
                request.Notes,
                cancellationToken);

            if (!changed)
            {
                return Result<Response>.Failure(
                    Error.NotFound("Admission application was not found."));
            }

            return Result<Response>.Success(
                new Response(request.Id, request.Status.ToDatabaseValue()));
        }

        private async Task<Result<Response>> AcceptAdmissionAsync(
            Guid tenantId,
            Request request,
            CancellationToken cancellationToken)
        {
            var application = await query.GetApplicationAsync(
                tenantId,
                request.Id,
                cancellationToken);

            if (application is null)
            {
                return Result<Response>.Failure(
                    Error.NotFound("Admission application was not found."));
            }

            if (application.StudentId.HasValue)
            {
                return Result<Response>.Failure(
                    Error.Conflict("This application has already been admitted."));
            }

            if (string.IsNullOrWhiteSpace(application.Email) ||
                string.IsNullOrWhiteSpace(application.GuardianEmail))
            {
                return Result<Response>.Failure(
                    Error.Validation(
                        "Student and guardian email are required before admission is accepted."));
            }

            var branchCode = await query.GetBranchCodeAsync(
                tenantId,
                application.BranchId,
                cancellationToken);

            if (string.IsNullOrWhiteSpace(branchCode))
            {
                return Result<Response>.Failure(
                    Error.Validation("Application branch is invalid."));
            }

            var studentId = Guid.NewGuid();
            var guardianId = Guid.NewGuid();
            var studentNumber = await numbers.NextAsync(
                $"STUDENT:{application.BranchId}",
                $"{branchCode}-",
                tenantId,
                7,
                cancellationToken);

            var studentAccount = await accounts.CreateAccountAsync(
                tenantId,
                studentId,
                SmartSchoolRoles.Student,
                application.Email,
                application.FirstName,
                application.LastName ?? string.Empty,
                application.SchoolId,
                application.BranchId,
                [SmartSchoolRoles.Student],
                cancellationToken);

            ProvisionedAccount? parentAccount = null;

            try
            {
                parentAccount = await accounts.CreateAccountAsync(
                    tenantId,
                    guardianId,
                    SmartSchoolRoles.Parent,
                    application.GuardianEmail,
                    application.GuardianName,
                    string.Empty,
                    application.SchoolId,
                    application.BranchId,
                    [SmartSchoolRoles.Parent],
                    cancellationToken);

                await command.CompleteAdmissionAsync(
                    tenantId,
                    application,
                    studentId,
                    studentAccount.UserId,
                    guardianId,
                    parentAccount.UserId,
                    studentNumber,
                    request.Notes,
                    cancellationToken);
            }
            catch
            {
                await accounts.DeactivateAccountAsync(
                    studentAccount.UserId,
                    cancellationToken);

                if (parentAccount is not null)
                {
                    await accounts.DeactivateAccountAsync(
                        parentAccount.UserId,
                        cancellationToken);
                }

                throw;
            }

            return Result<Response>.Success(
                new Response(
                    request.Id,
                    AdmissionApplicationStatus.AdmissionAccepted.ToDatabaseValue(),
                    studentNumber));
        }
    }
}

public static class GetAdmissionCriteria
{
    public sealed record Request(Guid? TenantId)
        : IRequest<Result<IReadOnlyList<AdmissionCriteriaDto>>>;

    public sealed class Handler(
        ITenantScope tenantScope,
        AdmissionWorkflowSlicesAdmissionWorkflowReadData query)
        : IRequestHandler<Request, Result<IReadOnlyList<AdmissionCriteriaDto>>>
    {
        public async Task<Result<IReadOnlyList<AdmissionCriteriaDto>>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<IReadOnlyList<AdmissionCriteriaDto>>.Failure(
                    Error.Validation("Tenant context is required."));
            }

            var criteria = await query.GetCriteriaAsync(
                tenantId.Value,
                cancellationToken);

            return Result<IReadOnlyList<AdmissionCriteriaDto>>.Success(criteria);
        }
    }
}

public static class CreateAdmissionCriteria
{
    public sealed record Request(
        Guid? TenantId,
        Guid SchoolId,
        Guid BranchId,
        Guid AcademicYearId,
        Guid ClassId,
        decimal MinimumMarks,
        decimal? EntranceTestMinimum,
        int? MinimumAge,
        int? MaximumAge,
        bool InterviewRequired,
        string? RequiredDocuments)
        : IRequest<Result<Response>>;

    public sealed record Response(Guid Id);

    public sealed class Handler(
        ITenantScope tenantScope,
        AdmissionWorkflowSlicesAdmissionWorkflowReadData query,
        AdmissionWorkflowSlicesAdmissionWorkflowWriteData command)
        : IRequestHandler<Request, Result<Response>>
    {
        public async Task<Result<Response>> HandleAsync(
            Request request,
            CancellationToken cancellationToken)
        {
            var tenantId = tenantScope.Resolve(request.TenantId);
            if (!tenantId.HasValue)
            {
                return Result<Response>.Failure(Error.Validation("Tenant context is required."));
            }

            var contextIsValid = await query.CriteriaContextIsValidAsync(
                tenantId.Value,
                request.SchoolId,
                request.BranchId,
                request.AcademicYearId,
                request.ClassId,
                cancellationToken);

            if (!contextIsValid)
            {
                return Result<Response>.Failure(
                    Error.Validation(
                        "School, branch, academic year and class must belong to the same tenant context."));
            }

            var criteriaId = await command.CreateCriteriaAsync(
                tenantId.Value,
                request,
                cancellationToken);

            return Result<Response>.Success(new Response(criteriaId));
        }
    }
}

/// <summary>
/// Feature-owned data access for AdmissionWorkflowSlices. Do not share across slices.
/// </summary>
public sealed class AdmissionWorkflowSlicesAdmissionWorkflowWriteData(IDbConnectionFactory connectionFactory)
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
}

/// <summary>
/// Feature-owned data access for AdmissionWorkflowSlices. Do not share across slices.
/// </summary>
public sealed class AdmissionWorkflowSlicesAdmissionWorkflowReadData(IDbConnectionFactory connectionFactory)
{
    public async Task<IReadOnlyList<AdmissionApplicationDto>> GetApplicationsAsync(
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                application_id AS Id,
                school_id AS SchoolId,
                branch_id AS BranchId,
                academic_year_id AS AcademicYearId,
                class_id AS ClassId,
                section_id AS SectionId,
                first_name AS FirstName,
                last_name AS LastName,
                date_of_birth AS DateOfBirth,
                gender AS Gender,
                email AS Email,
                phone AS Phone,
                guardian_name AS GuardianName,
                guardian_email AS GuardianEmail,
                guardian_phone AS GuardianPhone,
                previous_marks AS PreviousMarks,
                status AS Status,
                submitted_at AS SubmittedAt,
                decision_notes AS DecisionNotes,
                student_id AS StudentId
            FROM admission.student_application
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
            ORDER BY submitted_at DESC;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var command = new CommandDefinition(sql, new { TenantId = tenantId }, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<AdmissionApplicationDto>(command);
        return rows.AsList();
    }


    public async Task<AdmissionApplicationDetails?> GetApplicationAsync(
        Guid tenantId,
        Guid applicationId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                application_id AS Id,
                school_id AS SchoolId,
                branch_id AS BranchId,
                academic_year_id AS AcademicYearId,
                class_id AS ClassId,
                section_id AS SectionId,
                first_name AS FirstName,
                last_name AS LastName,
                date_of_birth AS DateOfBirth,
                gender AS Gender,
                email AS Email,
                guardian_name AS GuardianName,
                guardian_cnic AS GuardianCnic,
                guardian_email AS GuardianEmail,
                guardian_phone AS GuardianPhone,
                relationship AS Relationship,
                student_id AS StudentId
            FROM admission.student_application
            WHERE application_id = @ApplicationId
              AND tenant_id = @TenantId
              AND is_active = TRUE;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var command = new CommandDefinition(
            sql,
            new { ApplicationId = applicationId, TenantId = tenantId },
            cancellationToken: cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<AdmissionApplicationDetails>(command);
    }


    public async Task<IReadOnlyList<AdmissionCriteriaDto>> GetCriteriaAsync(
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                admission_criteria_id AS Id,
                school_id AS SchoolId,
                branch_id AS BranchId,
                academic_year_id AS AcademicYearId,
                class_id AS ClassId,
                minimum_marks AS MinimumMarks,
                entrance_test_minimum AS EntranceTestMinimum,
                minimum_age AS MinimumAge,
                maximum_age AS MaximumAge,
                interview_required AS InterviewRequired,
                required_documents AS RequiredDocuments,
                status AS Status
            FROM admission.admission_criteria
            WHERE tenant_id = @TenantId
            ORDER BY created_at DESC;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var command = new CommandDefinition(sql, new { TenantId = tenantId }, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<AdmissionCriteriaDto>(command);
        return rows.AsList();
    }


    public Task<bool> BranchBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid branchId, CancellationToken cancellationToken) =>
        ExistsAsync(
            """
            SELECT EXISTS (
                SELECT 1
                FROM org.campus
                WHERE tenant_id = @TenantId
                  AND school_id = @SchoolId
                  AND campus_id = @BranchId
                  AND is_active = TRUE
            );


    public async Task<string?> GetBranchGenderPolicyAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT g.code
            FROM org.campus c
            INNER JOIN reference.branch_gender_type g
                ON g.branch_gender_type_id = c.branch_gender_type_id
            WHERE c.tenant_id = @TenantId
              AND c.campus_id = @BranchId
              AND c.is_active = TRUE;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<string?>(new CommandDefinition(sql, new { TenantId = tenantId, BranchId = branchId }, cancellationToken: cancellationToken));
    }


    public Task<bool> ClassIsEligibleForBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid classId,
        CancellationToken cancellationToken) =>
        ExistsAsync(
            """
            SELECT EXISTS (
                SELECT 1
                FROM academic.class c
                INNER JOIN org.campus_education_level bel
                    ON bel.campus_id = c.branch_id
                   AND bel.education_level_id = c.education_level_id
                WHERE c.tenant_id = @TenantId
                  AND c.branch_id = @BranchId
                  AND c.class_id = @ClassId
                  AND c.is_active = TRUE
            );


    public Task<bool> AcademicYearBelongsToBranchAsync(Guid tenantId, Guid branchId, Guid academicYearId, CancellationToken cancellationToken) =>
        ExistsAsync(
            """
            SELECT EXISTS (
                SELECT 1
                FROM academic.academic_year
                WHERE tenant_id = @TenantId
                  AND branch_id = @BranchId
                  AND academic_year_id = @AcademicYearId
                  AND is_active = TRUE
            );


    public Task<bool> CriteriaContextIsValidAsync(Guid tenantId, Guid schoolId, Guid branchId, Guid academicYearId, Guid classId, CancellationToken cancellationToken) =>
        ExistsAsync(
            """
            SELECT EXISTS (
                SELECT 1
                FROM academic.class AS c
                INNER JOIN academic.academic_year AS y
                    ON y.branch_id = c.branch_id
                   AND y.tenant_id = c.tenant_id
                INNER JOIN org.campus AS b
                    ON b.campus_id = c.branch_id
                   AND b.tenant_id = c.tenant_id
                INNER JOIN org.campus_education_level AS bel
                    ON bel.campus_id = c.branch_id
                   AND bel.education_level_id = c.education_level_id
                WHERE c.tenant_id = @TenantId
                  AND b.school_id = @SchoolId
                  AND c.branch_id = @BranchId
                  AND c.class_id = @ClassId
                  AND y.academic_year_id = @AcademicYearId
                  AND c.is_active = TRUE
                  AND y.is_active = TRUE
                  AND b.is_active = TRUE
            );


    public async Task<string?> GetBranchCodeAsync(Guid tenantId, Guid branchId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT code
            FROM org.campus
            WHERE tenant_id = @TenantId
              AND campus_id = @BranchId
              AND is_active = TRUE;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<string?>(
            new CommandDefinition(sql, new { TenantId = tenantId, BranchId = branchId }, cancellationToken: cancellationToken));
    }
}
