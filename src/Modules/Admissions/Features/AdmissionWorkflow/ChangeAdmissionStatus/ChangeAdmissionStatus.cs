using SmartSchool.Application.Http;
using Dapper;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Application.Identity;
using SmartSchool.Application.Messaging;
using SmartSchool.SharedKernel;
using SmartSchool.SharedKernel.Constants;
using SmartSchool.Modules.Admissions.Persistence;

namespace SmartSchool.Modules.Admissions.Features;

public interface IChangeAdmissionStatusQuery
{
    Task<string?> ValidateAcceptanceAsync(Guid tenantId, AdmissionApplicationDetails application,
        decimal? entranceMarks, bool? interviewPassed, CancellationToken cancellationToken);

    Task<AdmissionApplicationDetails?> GetApplicationAsync(
        Guid tenantId,
        Guid applicationId,
        CancellationToken cancellationToken);

    Task<string?> GetBranchCodeAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken);
}

public sealed class ChangeAdmissionStatusQuery(IDbConnectionFactory connectionFactory, ICurrentUser user)
    : IChangeAdmissionStatusQuery
{
    public sealed record Policy(decimal MinimumMarks, decimal? EntranceTestMinimum, int? MinimumAge,
        int? MaximumAge, bool InterviewRequired, string? RequiredDocuments);
    public sealed record Placement(bool Allowed, DateOnly StartDate);

    public async Task<string?> ValidateAcceptanceAsync(Guid tenantId, AdmissionApplicationDetails application,
        decimal? entranceMarks, bool? interviewPassed, CancellationToken cancellationToken)
    {
        if (!application.AcademicYearId.HasValue || !application.ClassId.HasValue || !application.ClassSectionId.HasValue)
            return "Class, section and academic year are required before acceptance.";
        const string placementSql = """
            SELECT EXISTS (SELECT 1 FROM academic.class_section cs
                JOIN academic.grade_level gl ON gl.grade_level_id = cs.grade_level_id AND gl.tenant_id = cs.tenant_id AND gl.is_active
                JOIN org.campus c ON c.campus_id = cs.campus_id AND c.tenant_id = cs.tenant_id AND c.is_active
                JOIN reference.branch_gender_type gender ON gender.branch_gender_type_id = c.branch_gender_type_id
                JOIN org.campus_education_level level ON level.campus_id = c.campus_id AND level.tenant_id = c.tenant_id AND level.education_level_id = gl.education_level_id
                WHERE cs.tenant_id = @TenantId AND cs.class_section_id = @ClassSectionId AND cs.grade_level_id = @ClassId
                    AND cs.academic_year_id = @AcademicYearId AND cs.campus_id = @BranchId AND c.school_id = @SchoolId AND cs.is_active
                    AND (cs.capacity IS NULL OR cs.capacity > (SELECT count(*) FROM student.student_enrollment en WHERE en.tenant_id = cs.tenant_id AND en.class_section_id = cs.class_section_id AND en.is_active AND en.status = 'ACTIVE'))
                    AND (gender.code = 'CO_EDUCATION' OR (gender.code = 'BOYS_ONLY' AND upper(@Gender) IN ('MALE','BOY')) OR (gender.code = 'GIRLS_ONLY' AND upper(@Gender) IN ('FEMALE','GIRL')))) AS "Allowed",
                y.start_date AS "StartDate" FROM academic.academic_year y WHERE y.tenant_id = @TenantId AND y.academic_year_id = @AcademicYearId AND y.campus_id = @BranchId AND y.is_active;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var parameters = new { TenantId = tenantId, application.ClassSectionId, application.ClassId, application.AcademicYearId,
            application.BranchId, application.SchoolId, application.Gender, ApplicationId = application.Id };
        var placement = await connection.QuerySingleOrDefaultAsync<Placement>(new CommandDefinition(placementSql, parameters, cancellationToken: cancellationToken));
        if (placement is null || !placement.Allowed) return "Placement is invalid, full, or does not meet the branch's gender and education policy.";
        const string criteriaSql = """
            SELECT minimum_marks AS "MinimumMarks", entrance_test_minimum AS "EntranceTestMinimum", minimum_age AS "MinimumAge",
                maximum_age AS "MaximumAge", interview_required AS "InterviewRequired", required_documents AS "RequiredDocuments"
            FROM admission.admission_criteria WHERE tenant_id = @TenantId AND branch_id = @BranchId AND academic_year_id = @AcademicYearId
                AND class_id = @ClassId AND status = 'ACTIVE' ORDER BY created_at DESC LIMIT 1;
            """;
        var policy = await connection.QuerySingleOrDefaultAsync<Policy>(new CommandDefinition(criteriaSql, parameters, cancellationToken: cancellationToken));
        if (policy is not null)
        {
            if (policy.MinimumMarks > 0 && (!application.PreviousMarks.HasValue || application.PreviousMarks < policy.MinimumMarks)) return "Previous marks do not meet the configured minimum.";
            if (policy.EntranceTestMinimum.HasValue && (!entranceMarks.HasValue || entranceMarks < policy.EntranceTestMinimum)) return "Record an entrance-test score that meets the configured minimum.";
            if (entranceMarks is < 0 or > 100) return "Entrance-test marks must be between 0 and 100.";
            if (policy.InterviewRequired && interviewPassed != true) return "A passed interview must be recorded before acceptance.";
            if (policy.MinimumAge.HasValue || policy.MaximumAge.HasValue)
            {
                if (!application.DateOfBirth.HasValue) return "Date of birth is required by the age policy.";
                var reference = placement.StartDate;
                var age = reference.Year - application.DateOfBirth.Value.Year;
                if (application.DateOfBirth.Value.AddYears(age) > reference) age--;
                if (age < policy.MinimumAge || age > policy.MaximumAge) return "The applicant does not meet the age policy at the start of the academic year.";
            }
        }
        const string missingSql = """
            SELECT rt.name FROM document.required_document r
            JOIN document.required_document_type rt ON rt.required_document_type_id = r.required_document_type_id AND rt.tenant_id = r.tenant_id AND rt.is_active
            WHERE r.tenant_id = @TenantId AND upper(r.user_role) = 'STUDENT' AND r.is_mandatory AND r.is_active
                AND (r.campus_id IS NULL OR r.campus_id = @BranchId)
                AND NOT EXISTS (SELECT 1 FROM document.document d WHERE d.tenant_id = @TenantId AND d.owner_type = 'AdmissionDocument'
                    AND d.owner_id = @ApplicationId AND d.required_document_type_id = rt.required_document_type_id AND d.is_active AND d.status = 'ACTIVE');
            """;
        var missing = (await connection.QueryAsync<string>(new CommandDefinition(missingSql, parameters, cancellationToken: cancellationToken))).ToList();
        foreach (var code in (policy?.RequiredDocuments ?? string.Empty).Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
        {
            const string documentSql = """
                SELECT EXISTS (SELECT 1 FROM document.document d
                    LEFT JOIN document.required_document_type rt ON rt.required_document_type_id = d.required_document_type_id AND rt.tenant_id = d.tenant_id
                    JOIN document.document_type dt ON dt.document_type_id = d.document_type_id AND dt.tenant_id = d.tenant_id
                    WHERE d.tenant_id = @TenantId AND d.owner_type = 'AdmissionDocument' AND d.owner_id = @ApplicationId AND d.is_active AND d.status = 'ACTIVE'
                        AND (upper(rt.code) = upper(@Code) OR upper(dt.code) = upper(@Code)));
                """;
            if (!await connection.ExecuteScalarAsync<bool>(new CommandDefinition(documentSql, new { TenantId = tenantId, ApplicationId = application.Id, Code = code }, cancellationToken: cancellationToken))) missing.Add(code);
        }
        return missing.Count > 0 ? $"Required application documents are missing: {string.Join(", ", missing.Distinct())}." : null;
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
                class_section_id AS ClassSectionId,
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
                student_id AS StudentId, previous_marks AS PreviousMarks
            FROM admission.student_application
            WHERE application_id = @ApplicationId
                AND tenant_id = @TenantId
                AND is_active = TRUE AND (@ScopeBranchId IS NULL OR branch_id = @ScopeBranchId);
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<AdmissionApplicationDetails>(
            new CommandDefinition(
                sql,
                new
                {
                    ScopeBranchId = user.BranchId,
                    ApplicationId = applicationId,
                    TenantId = tenantId
                },
                cancellationToken: cancellationToken));
    }

    public async Task<string?> GetBranchCodeAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT code
            FROM org.campus
            WHERE tenant_id = @TenantId
                AND campus_id = @BranchId
                AND is_active = TRUE;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        return await connection.ExecuteScalarAsync<string?>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    BranchId = branchId
                },
                cancellationToken: cancellationToken));
    }
}

public interface IChangeAdmissionStatusCommand
{
    Task<bool> ChangeStatusAsync(
        Guid tenantId,
        Guid applicationId,
        AdmissionApplicationStatus status,
        string? notes,
        CancellationToken cancellationToken);
}

public sealed class ChangeAdmissionStatusCommand(
    IAdmissionsDbContext dbContext,
    TimeProvider timeProvider) : IChangeAdmissionStatusCommand
{
    public async Task<bool> ChangeStatusAsync(
        Guid tenantId,
        Guid applicationId,
        AdmissionApplicationStatus status,
        string? notes,
        CancellationToken cancellationToken)
    {
        var application = await dbContext.AdmissionApplications
            .SingleOrDefaultAsync(
                entity => entity.ApplicationId == applicationId &&
                          entity.TenantId == tenantId,
                cancellationToken);

        if (application is null)
        {
            return false;
        }

        application.ChangeStatus(
            status,
            notes,
            timeProvider.GetUtcNow().UtcDateTime);

        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}

public static class ChangeAdmissionStatus
{
    public sealed record Body(
        Guid? TenantId,
        string Status,
        string? Notes, decimal? EntranceTestMarks = null, bool? InterviewPassed = null);

    public sealed record Request(
        Guid Id,
        Guid? TenantId,
        AdmissionApplicationStatus Status,
        string? Notes, decimal? EntranceTestMarks = null, bool? InterviewPassed = null)
        : IRequest<Result<Response>>;

    public sealed record Response(
        Guid Id,
        string Status,
        string? StudentNumber = null);

    public sealed class Handler(
        ITenantScope tenantScope,
        IChangeAdmissionStatusQuery query,
        IChangeAdmissionStatusCommand command,
        ICompleteAdmission completeAdmission,
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
            var application = await query.GetApplicationAsync(tenantId, request.Id, cancellationToken);
            if (application?.StudentId is not null) return Result<Response>.Failure(Error.Conflict("An admitted application cannot be changed."));
            if (application is null) return Result<Response>.Failure(Error.NotFound("Admission application was not found."));
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

            var validationError = await query.ValidateAcceptanceAsync(tenantId, application, request.EntranceTestMarks, request.InterviewPassed, cancellationToken);
            if (validationError is not null) return Result<Response>.Failure(Error.Validation(validationError));

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

                await completeAdmission.ExecuteAsync(
                    tenantId,
                    application,
                    studentId,
                    studentAccount.UserId,
                    guardianId,
                    parentAccount.UserId,
                    studentNumber,
                    request.Notes,
                    request.EntranceTestMarks,
                    request.InterviewPassed,
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

    public static void MapEndpoint(IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut("/api/admissions/workflow/applications/{id:guid}/status", async (Guid id, Body body, IMediator mediator, CancellationToken cancellationToken) =>
        {
            if (!AdmissionApplicationStatusExtensions.TryParseDatabaseValue(body.Status, out var status))
                return Results.BadRequest(new { message = "Invalid admission status." });
            return (await mediator.SendAsync<Request, Result<Response>>(new Request(id, body.TenantId, status, body.Notes, body.EntranceTestMarks, body.InterviewPassed), cancellationToken)).ToHttpResult();
        }).WithName("ChangeAdmissionStatus").WithTags("Admissions").RequireAuthorization(SmartSchoolPolicies.SchoolAdministration);
    }
}

public sealed record AdmissionApplicationDetails(
    Guid Id,
    Guid SchoolId,
    Guid BranchId,
    Guid? AcademicYearId,
    Guid? ClassId,
    Guid? ClassSectionId,
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
    Guid? StudentId,
    decimal? PreviousMarks);
