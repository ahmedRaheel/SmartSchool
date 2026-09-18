using System.Data.Common;
using Dapper;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Features;

namespace SmartSchool.Api.Integration;

/// <summary>
/// Composition-boundary adapter for Admissions. Cross-module data access is intentionally
/// kept outside every business module so modules remain independently extractable.
/// </summary>
public sealed class AdmissionsExternalPortAdapter(IDbConnectionFactory connectionFactory)
    : IAdmissionsExternalPort
{
    public Task<bool> CriteriaContextIsValidAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        Guid academicYearId,
        Guid classId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM academic.grade_level AS class
                INNER JOIN academic.academic_year AS academic_year
                    ON academic_year.campus_id = class.campus_id
                   AND academic_year.tenant_id = class.tenant_id
                INNER JOIN org.campus AS branch
                    ON branch.campus_id = class.campus_id
                   AND branch.tenant_id = class.tenant_id
                INNER JOIN org.campus_education_level AS branch_level
                    ON branch_level.campus_id = class.campus_id
                   AND branch_level.education_level_id = class.education_level_id
                WHERE class.tenant_id = @TenantId
                  AND branch.school_id = @SchoolId
                  AND class.campus_id = @BranchId
                  AND class.grade_level_id = @ClassId
                  AND academic_year.academic_year_id = @AcademicYearId
                  AND class.is_active = TRUE
                  AND academic_year.is_active = TRUE
                  AND branch.is_active = TRUE
            );
            """;

        return ExecuteExistsAsync(
            sql,
            new
            {
                TenantId = tenantId,
                SchoolId = schoolId,
                BranchId = branchId,
                AcademicYearId = academicYearId,
                ClassId = classId
            },
            cancellationToken);
    }

    public Task<bool> BranchBelongsToSchoolAsync(
        Guid tenantId,
        Guid schoolId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM org.campus
                WHERE tenant_id = @TenantId
                  AND school_id = @SchoolId
                  AND campus_id = @BranchId
                  AND is_active = TRUE
            );
            """;

        return ExecuteExistsAsync(
            sql,
            new { TenantId = tenantId, SchoolId = schoolId, BranchId = branchId },
            cancellationToken);
    }

    public async Task<string?> GetBranchGenderPolicyAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT gender_type.code
            FROM org.campus AS branch
            INNER JOIN reference.branch_gender_type AS gender_type
                ON gender_type.branch_gender_type_id = branch.branch_gender_type_id
            WHERE branch.tenant_id = @TenantId
              AND branch.campus_id = @BranchId
              AND branch.is_active = TRUE;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<string?>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, BranchId = branchId },
                cancellationToken: cancellationToken));
    }

    public Task<bool> ClassIsEligibleForBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid classId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM academic.grade_level AS class
                INNER JOIN org.campus_education_level AS branch_level
                    ON branch_level.campus_id = class.campus_id
                   AND branch_level.education_level_id = class.education_level_id
                WHERE class.tenant_id = @TenantId
                  AND class.campus_id = @BranchId
                  AND class.grade_level_id = @ClassId
                  AND class.is_active = TRUE
            );
            """;

        return ExecuteExistsAsync(
            sql,
            new { TenantId = tenantId, BranchId = branchId, ClassId = classId },
            cancellationToken);
    }

    public Task<bool> AcademicYearBelongsToBranchAsync(
        Guid tenantId,
        Guid branchId,
        Guid academicYearId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM academic.academic_year
                WHERE tenant_id = @TenantId
                  AND campus_id = @BranchId
                  AND academic_year_id = @AcademicYearId
                  AND is_active = TRUE
            );
            """;

        return ExecuteExistsAsync(
            sql,
            new
            {
                TenantId = tenantId,
                BranchId = branchId,
                AcademicYearId = academicYearId
            },
            cancellationToken);
    }

    public Task<bool> PlacementIsValidAsync(
        Guid tenantId,
        Guid branchId,
        Guid academicYearId,
        Guid classId,
        Guid classSectionId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM academic.class_section AS section
                WHERE section.tenant_id = @TenantId
                  AND section.campus_id = @BranchId
                  AND section.academic_year_id = @AcademicYearId
                  AND section.grade_level_id = @ClassId
                  AND section.class_section_id = @ClassSectionId
                  AND section.is_active = TRUE
            );
            """;

        return ExecuteExistsAsync(
            sql,
            new
            {
                TenantId = tenantId,
                BranchId = branchId,
                AcademicYearId = academicYearId,
                ClassId = classId,
                ClassSectionId = classSectionId
            },
            cancellationToken);
    }

    public async Task<AdmissionPlacementValidation?> ValidatePlacementAsync(
        Guid tenantId,
        AdmissionApplicationDetails application,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                EXISTS (
                    SELECT 1
                    FROM academic.class_section AS section
                    INNER JOIN academic.grade_level AS class
                        ON class.grade_level_id = section.grade_level_id
                       AND class.tenant_id = section.tenant_id
                       AND class.is_active = TRUE
                    INNER JOIN org.campus AS branch
                        ON branch.campus_id = section.campus_id
                       AND branch.tenant_id = section.tenant_id
                       AND branch.is_active = TRUE
                    INNER JOIN reference.branch_gender_type AS gender
                        ON gender.branch_gender_type_id = branch.branch_gender_type_id
                    INNER JOIN org.campus_education_level AS level
                        ON level.campus_id = branch.campus_id
                       AND level.tenant_id = branch.tenant_id
                       AND level.education_level_id = class.education_level_id
                    WHERE section.tenant_id = @TenantId
                      AND section.class_section_id = @ClassSectionId
                      AND section.grade_level_id = @ClassId
                      AND section.academic_year_id = @AcademicYearId
                      AND section.campus_id = @BranchId
                      AND branch.school_id = @SchoolId
                      AND section.is_active = TRUE
                      AND (
                          section.capacity IS NULL
                          OR section.capacity > (
                              SELECT count(*)
                              FROM student.student_enrollment AS enrollment
                              WHERE enrollment.tenant_id = section.tenant_id
                                AND enrollment.class_section_id = section.class_section_id
                                AND enrollment.is_active = TRUE
                                AND enrollment.status = 'ACTIVE'
                          )
                      )
                      AND (
                          gender.code = 'CO_EDUCATION'
                          OR (gender.code = 'BOYS_ONLY' AND upper(@Gender) IN ('MALE', 'BOY'))
                          OR (gender.code = 'GIRLS_ONLY' AND upper(@Gender) IN ('FEMALE', 'GIRL'))
                      )
                ) AS "Allowed",
                academic_year.start_date AS "StartDate"
            FROM academic.academic_year AS academic_year
            WHERE academic_year.tenant_id = @TenantId
              AND academic_year.academic_year_id = @AcademicYearId
              AND academic_year.campus_id = @BranchId
              AND academic_year.is_active = TRUE;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<AdmissionPlacementValidation>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    application.ClassSectionId,
                    application.ClassId,
                    application.AcademicYearId,
                    application.BranchId,
                    application.SchoolId,
                    application.Gender
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

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<string?>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, BranchId = branchId },
                cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyCollection<string>> GetMissingApplicationDocumentsAsync(
        Guid tenantId,
        Guid applicationId,
        Guid branchId,
        IReadOnlyCollection<string> requiredDocumentCodes,
        CancellationToken cancellationToken)
    {
        const string mandatorySql = """
            SELECT required_type.name
            FROM document.required_document AS requirement
            INNER JOIN document.required_document_type AS required_type
                ON required_type.required_document_type_id = requirement.required_document_type_id
               AND required_type.tenant_id = requirement.tenant_id
               AND required_type.is_active = TRUE
            WHERE requirement.tenant_id = @TenantId
              AND upper(requirement.user_role) = 'STUDENT'
              AND requirement.is_mandatory = TRUE
              AND requirement.is_active = TRUE
              AND (requirement.campus_id IS NULL OR requirement.campus_id = @BranchId)
              AND NOT EXISTS (
                  SELECT 1
                  FROM document.document AS document
                  WHERE document.tenant_id = @TenantId
                    AND document.owner_type = 'AdmissionDocument'
                    AND document.owner_id = @ApplicationId
                    AND document.required_document_type_id = required_type.required_document_type_id
                    AND document.is_active = TRUE
                    AND document.status = 'ACTIVE'
              );
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var missing = (await connection.QueryAsync<string>(
            new CommandDefinition(
                mandatorySql,
                new { TenantId = tenantId, ApplicationId = applicationId, BranchId = branchId },
                cancellationToken: cancellationToken))).ToList();

        const string configuredSql = """
            SELECT EXISTS (
                SELECT 1
                FROM document.document AS document
                LEFT JOIN document.required_document_type AS required_type
                    ON required_type.required_document_type_id = document.required_document_type_id
                   AND required_type.tenant_id = document.tenant_id
                INNER JOIN document.document_type AS document_type
                    ON document_type.document_type_id = document.document_type_id
                   AND document_type.tenant_id = document.tenant_id
                WHERE document.tenant_id = @TenantId
                  AND document.owner_type = 'AdmissionDocument'
                  AND document.owner_id = @ApplicationId
                  AND document.is_active = TRUE
                  AND document.status = 'ACTIVE'
                  AND (
                      upper(required_type.code) = upper(@Code)
                      OR upper(document_type.code) = upper(@Code)
                  )
            );
            """;

        foreach (var code in requiredDocumentCodes)
        {
            var exists = await connection.ExecuteScalarAsync<bool>(
                new CommandDefinition(
                    configuredSql,
                    new { TenantId = tenantId, ApplicationId = applicationId, Code = code },
                    cancellationToken: cancellationToken));

            if (!exists)
            {
                missing.Add(code);
            }
        }

        return missing.Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
    }

    public async Task<ClassSectionAvailability?> GetClassSectionAvailabilityAsync(
        Guid tenantId,
        Guid classSectionId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                capacity AS "Capacity",
                is_active AS "IsActive"
            FROM academic.class_section
            WHERE tenant_id = @TenantId
              AND class_section_id = @ClassSectionId;
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<ClassSectionAvailability>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, ClassSectionId = classSectionId },
                cancellationToken: cancellationToken));
    }

    public async Task<int> GetActiveEnrollmentCountAsync(
        Guid tenantId,
        Guid classSectionId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT count(*)::int
            FROM student.student_enrollment
            WHERE tenant_id = @TenantId
              AND class_section_id = @ClassSectionId
              AND is_active = TRUE
              AND status = 'ACTIVE';
            """;

        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                sql,
                new { TenantId = tenantId, ClassSectionId = classSectionId },
                cancellationToken: cancellationToken));
    }

    public async Task ProvisionAdmissionAsync(
        AdmissionProvisioningRequest request,
        CancellationToken cancellationToken)
    {
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        try
        {
            var now = DateTimeOffset.UtcNow;
            var rowVersion = Array.Empty<byte>();

            const string studentSql = """
                INSERT INTO student.student (
                    student_id, tenant_id, user_id, school_id, branch_id, student_number,
                    first_name, last_name, date_of_birth, gender, admission_date, status,
                    is_active, created_at, row_version)
                VALUES (
                    @StudentId, @TenantId, @StudentUserId, @SchoolId, @BranchId, @StudentNumber,
                    @FirstName, @LastName, @DateOfBirth, @Gender, @AdmissionDate, 'ACTIVE',
                    TRUE, @Now, @RowVersion);
                """;

            await connection.ExecuteAsync(
                new CommandDefinition(
                    studentSql,
                    new
                    {
                        request.StudentId,
                        request.TenantId,
                        request.StudentUserId,
                        request.SchoolId,
                        request.BranchId,
                        request.StudentNumber,
                        request.FirstName,
                        request.LastName,
                        request.DateOfBirth,
                        request.Gender,
                        request.AdmissionDate,
                        Now = now,
                        RowVersion = rowVersion
                    },
                    transaction,
                    cancellationToken: cancellationToken));

            const string guardianSql = """
                INSERT INTO student.guardian (
                    guardian_id, tenant_id, user_id, full_name, cnic_number, email, phone,
                    is_active, created_at, row_version)
                VALUES (
                    @GuardianId, @TenantId, @GuardianUserId, @GuardianName, @GuardianCnic,
                    @GuardianEmail, @GuardianPhone, TRUE, @Now, @RowVersion);
                """;

            await connection.ExecuteAsync(
                new CommandDefinition(
                    guardianSql,
                    new
                    {
                        request.GuardianId,
                        request.TenantId,
                        request.GuardianUserId,
                        request.GuardianName,
                        request.GuardianCnic,
                        request.GuardianEmail,
                        request.GuardianPhone,
                        Now = now,
                        RowVersion = rowVersion
                    },
                    transaction,
                    cancellationToken: cancellationToken));

            const string linkSql = """
                INSERT INTO student.student_guardian (
                    student_guardian_id, tenant_id, student_id, guardian_id, relationship,
                    is_primary, can_view_academics, can_view_finance, can_pickup,
                    is_active, created_at)
                VALUES (
                    @StudentGuardianId, @TenantId, @StudentId, @GuardianId, @Relationship,
                    TRUE, TRUE, TRUE, FALSE, TRUE, @Now);
                """;

            await connection.ExecuteAsync(
                new CommandDefinition(
                    linkSql,
                    new
                    {
                        StudentGuardianId = Guid.NewGuid(),
                        request.TenantId,
                        request.StudentId,
                        request.GuardianId,
                        request.Relationship,
                        Now = now
                    },
                    transaction,
                    cancellationToken: cancellationToken));

            if (request.AcademicYearId.HasValue && request.ClassSectionId.HasValue)
            {
                const string enrollmentSql = """
                    INSERT INTO student.student_enrollment (
                        student_enrollment_id, tenant_id, student_id, enrollment_number,
                        academic_year_id, class_section_id, enrollment_date, status,
                        is_active, created_at, row_version)
                    VALUES (
                        @StudentEnrollmentId, @TenantId, @StudentId, @EnrollmentNumber,
                        @AcademicYearId, @ClassSectionId, @AdmissionDate, 'ACTIVE',
                        TRUE, @Now, @RowVersion);
                    """;

                await connection.ExecuteAsync(
                    new CommandDefinition(
                        enrollmentSql,
                        new
                        {
                            StudentEnrollmentId = Guid.NewGuid(),
                            request.TenantId,
                            request.StudentId,
                            EnrollmentNumber = "ENR-" + request.StudentNumber,
                            request.AcademicYearId,
                            request.ClassSectionId,
                            request.AdmissionDate,
                            Now = now,
                            RowVersion = rowVersion
                        },
                        transaction,
                        cancellationToken: cancellationToken));
            }

            const string documentsSql = """
                UPDATE document.document
                SET owner_id = @StudentId,
                    owner_type = 'StudentDocument',
                    updated_at = @Now
                WHERE tenant_id = @TenantId
                  AND owner_id = @ApplicationId
                  AND owner_type = 'AdmissionDocument';
                """;

            await connection.ExecuteAsync(
                new CommandDefinition(
                    documentsSql,
                    new
                    {
                        request.StudentId,
                        request.TenantId,
                        request.ApplicationId,
                        Now = now
                    },
                    transaction,
                    cancellationToken: cancellationToken));

            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task<bool> ExecuteExistsAsync(
        string sql,
        object parameters,
        CancellationToken cancellationToken)
    {
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                parameters,
                cancellationToken: cancellationToken));
    }
}
