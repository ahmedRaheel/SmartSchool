using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Library.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Library.Features.Book;

/// <summary>
/// Executes database reads for <see cref="BookEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class BookQuery(IDbConnectionFactory connectionFactory) : IBookQuery
{
    public async Task<BookEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT *
            FROM library.book
            WHERE tenant_id = @TenantId
              AND book_id = @Id
              AND is_active = TRUE;
            """;

        await using var connection =
            await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

        return await connection.QuerySingleOrDefaultAsync<BookEntity>(
            new CommandDefinition(
                sql,
                new
                {
                    TenantId = tenantId,
                    Id = id
                },
                cancellationToken: cancellationToken)).ConfigureAwait(false);
    }

    public async Task<PagedResult<BookEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        const string countSql = """
            SELECT COUNT(*)
            FROM library.book
            WHERE tenant_id = @TenantId
              AND is_active = TRUE;
            """;

        const string pageSql = """
            SELECT
                tenant_id AS "TenantId",
                book_id AS "Id"
            FROM library.book
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
            ORDER BY book_id
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

        var items = (await connection.QueryAsync<BookEntity>(
            new CommandDefinition(
                pageSql,
                parameters,
                cancellationToken: cancellationToken)).ConfigureAwait(false))
            .AsList();

        return new PagedResult<BookEntity>(
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
                FROM library.book
                WHERE tenant_id = @TenantId
                  AND code = @Code
                  AND (@ExcludingId IS NULL OR book_id <> @ExcludingId)
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
