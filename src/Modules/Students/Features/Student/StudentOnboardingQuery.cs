using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Students.Features.Student;

public sealed class StudentOnboardingQuery(IDbConnectionFactory connectionFactory) : IStudentOnboardingQuery
{
    public async Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM org.campus
                WHERE tenant_id = @TenantId
                  AND school_id = @SchoolId
                  AND campus_id = @CampusId
            );
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, SchoolId = schoolId, CampusId = campusId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> HasGuardianAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM student.student_guardian WHERE tenant_id=@TenantId AND student_id=@StudentId AND is_active=true);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId }, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<string>> GetMissingRequiredDocumentsAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT r.display_name
            FROM document.required_document r
            WHERE r.is_active = true
              AND r.is_required = true
              AND r.actor_type = 'STUDENT'
              AND (r.tenant_id IS NULL OR r.tenant_id = @TenantId)
              AND NOT EXISTS (
                  SELECT 1
                  FROM document.document d
                  JOIN document.student_document sd ON sd.document_id = d.document_id
                  WHERE sd.tenant_id = @TenantId
                    AND sd.student_id = @StudentId
                    AND d.document_type = r.document_type
                    AND d.status = 'ACTIVE'
              );
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<string>(new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId }, cancellationToken: cancellationToken));
        return rows.AsList();
    }

    public async Task<AdmissionPlacementReadModel?> GetPendingPlacementAsync(Guid tenantId, Guid studentId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT ap.academic_year_id AS AcademicYearId,
                   ap.class_section_id AS ClassSectionId,
                   cs.class_id AS ClassId
            FROM student.admission_placement ap
            JOIN academic.class_section cs ON cs.class_section_id = ap.class_section_id AND cs.tenant_id = ap.tenant_id
            WHERE ap.tenant_id = @TenantId
              AND ap.student_id = @StudentId
              AND ap.status = 'PENDING'
            ORDER BY ap.requested_at DESC
            LIMIT 1;
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<AdmissionPlacementReadModel>(new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId }, cancellationToken: cancellationToken));
    }

    public async Task<string?> GetCampusCodeAsync(Guid tenantId, Guid campusId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT code FROM org.campus WHERE tenant_id=@TenantId AND campus_id=@CampusId;";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<string?>(new CommandDefinition(sql, new { TenantId = tenantId, CampusId = campusId }, cancellationToken: cancellationToken));
    }

    public async Task<bool> StudentAndGuardianBelongToTenantAsync(Guid tenantId, Guid studentId, Guid guardianId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM student.student s
                JOIN student.guardian g ON g.tenant_id = s.tenant_id
                WHERE s.tenant_id = @TenantId
                  AND s.student_id = @StudentId
                  AND g.guardian_id = @GuardianId
            );
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, StudentId = studentId, GuardianId = guardianId }, cancellationToken: cancellationToken));
    }
}
