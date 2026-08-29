using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.HR.Features.DataAccess.EmployeeOnboarding;

public sealed class EmployeeOnboardingQuery(IDbConnectionFactory connectionFactory) : IEmployeeOnboardingQuery
{
    public async Task<bool> CampusBelongsToSchoolAsync(Guid tenantId, Guid schoolId, Guid campusId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM org.campus WHERE tenant_id=@TenantId AND school_id=@SchoolId AND campus_id=@CampusId);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId=tenantId, SchoolId=schoolId, CampusId=campusId }, cancellationToken:cancellationToken));
    }

    public async Task<bool> DepartmentBelongsToCampusAsync(Guid tenantId, Guid campusId, Guid departmentId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM org.department WHERE tenant_id=@TenantId AND campus_id=@CampusId AND department_id=@DepartmentId AND is_active=true);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId=tenantId, CampusId=campusId, DepartmentId=departmentId }, cancellationToken:cancellationToken));
    }

    public async Task<IReadOnlyList<string>> GetMissingRequiredDocumentsAsync(Guid tenantId, Guid employeeId, string staffType, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT r.display_name
            FROM document.required_document r
            WHERE r.is_active=true AND r.is_required=true AND r.actor_type='EMPLOYEE'
              AND (r.tenant_id IS NULL OR r.tenant_id=@TenantId)
              AND (r.staff_type IS NULL OR r.staff_type=@StaffType)
              AND (r.condition_code IS NULL OR (r.condition_code='EXPERIENCE_PRESENT' AND EXISTS(
                    SELECT 1 FROM hr.employee_experience x WHERE x.tenant_id=@TenantId AND x.employee_id=@EmployeeId)))
              AND NOT EXISTS (
                    SELECT 1 FROM document.document d
                    LEFT JOIN document.teacher_document td ON td.document_id=d.document_id AND td.teacher_id=@EmployeeId
                    LEFT JOIN document.admin_officer_document ad ON ad.document_id=d.document_id AND ad.employee_id=@EmployeeId
                    LEFT JOIN document.staff_document sd ON sd.document_id=d.document_id AND sd.employee_id=@EmployeeId
                    WHERE d.tenant_id=@TenantId AND d.document_type=r.document_type AND d.status='ACTIVE'
                      AND (td.document_id IS NOT NULL OR ad.document_id IS NOT NULL OR sd.document_id IS NOT NULL));
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        var rows = await connection.QueryAsync<string>(new CommandDefinition(sql, new { TenantId=tenantId, EmployeeId=employeeId, StaffType=staffType }, cancellationToken:cancellationToken));
        return rows.AsList();
    }

    public async Task<bool> HasEducationAsync(Guid tenantId, Guid employeeId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT EXISTS(SELECT 1 FROM hr.employee_education WHERE tenant_id=@TenantId AND employee_id=@EmployeeId);";
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId=tenantId, EmployeeId=employeeId }, cancellationToken:cancellationToken));
    }
}
