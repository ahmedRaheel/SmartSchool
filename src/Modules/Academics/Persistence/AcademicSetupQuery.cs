using Dapper;
using SmartSchool.Application.Persistence;

namespace SmartSchool.Modules.Academics.Persistence;

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
                code AS Code,
                branch_id AS BranchId,
                start_date AS StartDate,
                end_date AS EndDate,
                is_current AS IsCurrent
            FROM academic.academic_year
            WHERE tenant_id = @TenantId
              AND branch_id = @BranchId
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
                branch_id AS BranchId
            FROM academic.class
            WHERE tenant_id = @TenantId
              AND branch_id = @BranchId
              AND is_active = TRUE
            ORDER BY sort_order, name;
            """;

        return QueryAsync(sql, tenantId, branchId, cancellationToken);
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
