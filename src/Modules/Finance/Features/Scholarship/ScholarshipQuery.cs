using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Features.Scholarship;

/// <summary>
/// Executes database reads for <see cref="ScholarshipEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class ScholarshipQuery(IDbConnectionFactory connectionFactory) : IScholarshipQuery
{
    public async Task<ScholarshipEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT *
            FROM finance.scholarship
            WHERE tenant_id = @TenantId
              AND scholarship_id = @Id
              AND is_active = TRUE;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

        return await connection.QuerySingleOrDefaultAsync<ScholarshipEntity>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    Id = id
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }

    public async Task<PagedResult<ScholarshipEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        const string countSql = """
            SELECT COUNT(*)
            FROM finance.scholarship
            WHERE tenant_id = @TenantId
              AND is_active = TRUE;
            """;

        const string pageSql = """
            SELECT
                tenant_id AS "TenantId",
                scholarship_id AS "Id"
            FROM finance.scholarship
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
            ORDER BY scholarship_id
            LIMIT @PageSize OFFSET @Offset;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken);

        var parameters = new
        {
            TenantId = tenantId,
            PageSize = pageSize,
            Offset = (page - 1) * pageSize
        };

        var totalCount = await connection.ExecuteScalarAsync<long>(
            new CommandDefinition(
                countSql,
                parameters,
                cancellationToken: cancellationToken)).ConfigureAwait(false);

        var items = (await connection.QueryAsync<ScholarshipEntity>(
            new CommandDefinition(
                pageSql,
                parameters,
                cancellationToken: cancellationToken)).ConfigureAwait(false))
            .AsList();

        return new PagedResult<ScholarshipEntity>(
            items,
            page,
            pageSize,
            totalCount);
    }

    public async Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT EXISTS (
                SELECT 1
                FROM finance.scholarship
                WHERE tenant_id = @TenantId
                  AND code = @Code
                  AND (@ExcludingId IS NULL OR scholarship_id <> @ExcludingId)
            );
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    Code = code,
                    ExcludingId = excludingId
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }
}
