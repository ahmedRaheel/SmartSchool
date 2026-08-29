using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Academics.Features.DataAccess.AcademicSetup;

public sealed class AcademicSetupQuery(
    IDbConnectionFactory connectionFactory) : IAcademicSetupQuery
{
    public Task<IReadOnlyCollection<AcademicSetupItem>> GetAcademicYearsAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                academic_year_id AS Id,
                name AS Name,
                replace(name, '/', '-') AS Code,
                campus_id AS BranchId,
                start_date AS StartDate,
                end_date AS EndDate,
                is_current AS IsCurrent
            FROM academic.academic_year
            WHERE tenant_id = @TenantId
              AND campus_id = @BranchId
              AND is_active = TRUE
            ORDER BY start_date DESC;
            """;

        return QueryAsync(sql, tenantId, branchId, cancellationToken);
    }

    public Task<IReadOnlyCollection<AcademicSetupItem>> GetClassesAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                class_id AS Id,
                name AS Name,
                code AS Code,
                c.branch_id AS BranchId,
                c.education_level_id AS EducationLevelId,
                l.name AS EducationLevelName
            FROM academic.class c
            LEFT JOIN reference.education_level l ON l.education_level_id = c.education_level_id
            WHERE c.tenant_id = @TenantId
              AND c.branch_id = @BranchId
              AND c.is_active = TRUE
            ORDER BY c.sort_order, c.name;
            """;

        return QueryAsync(sql, tenantId, branchId, cancellationToken);
    }

    public async Task<bool> BranchAllowsEducationLevelAsync(
        Guid tenantId,
        Guid branchId,
        Guid educationLevelId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM org.branch_education_level bel
                INNER JOIN org.campus c ON c.campus_id = bel.branch_id
                WHERE c.tenant_id = @TenantId
                  AND bel.branch_id = @BranchId
                  AND bel.education_level_id = @EducationLevelId
                  AND c.is_active = TRUE
            );
            """;
        await using var connection = await connectionFactory.OpenConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sql, new { TenantId = tenantId, BranchId = branchId, EducationLevelId = educationLevelId }, cancellationToken: cancellationToken));
    }

    public Task<IReadOnlyCollection<AcademicSetupItem>> GetSectionsAsync(
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT
                section_id AS Id,
                name AS Name,
                code AS Code,
                branch_id AS BranchId,
                class_id AS ParentId
            FROM academic.section
            WHERE tenant_id = @TenantId
              AND branch_id = @BranchId
              AND is_active = TRUE
            ORDER BY name;
            """;

        return QueryAsync(sql, tenantId, branchId, cancellationToken);
    }

    private async Task<IReadOnlyCollection<AcademicSetupItem>> QueryAsync(
        string sql,
        Guid tenantId,
        Guid branchId,
        CancellationToken cancellationToken)
    {
        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        var command = new CommandDefinition(
            sql,
            new
            {
                TenantId = tenantId,
                BranchId = branchId
            },
            cancellationToken: cancellationToken);

        var items = await connection.QueryAsync<AcademicSetupItem>(command);
        return items.AsList();
    }
}
