using Dapper;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Features;

namespace SmartSchool.Modules.Admissions.Features.DataAccess.AdmissionWorkflow;

internal sealed class AdmissionWorkflowQuery(IDbConnectionFactory connectionFactory)
    : IAdmissionWorkflowQuery
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
            """,
            new { TenantId = tenantId, SchoolId = schoolId, BranchId = branchId },
            cancellationToken);

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
            """,
            new { TenantId = tenantId, BranchId = branchId, ClassId = classId },
            cancellationToken);

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
            """,
            new { TenantId = tenantId, BranchId = branchId, AcademicYearId = academicYearId },
            cancellationToken);

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
            """,
            new
            {
                TenantId = tenantId,
                SchoolId = schoolId,
                BranchId = branchId,
                AcademicYearId = academicYearId,
                ClassId = classId
            },
            cancellationToken);

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

    private async Task<bool> ExistsAsync(string sql, object parameters, CancellationToken cancellationToken)
    {
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(sql, parameters, cancellationToken: cancellationToken));
    }
}
