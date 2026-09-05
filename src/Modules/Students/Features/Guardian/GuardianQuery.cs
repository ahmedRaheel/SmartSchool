using SmartSchool.Modules.Students.Persistence;
using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Guardian;

/// <summary>
/// Executes database reads for <see cref="GuardianEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class GuardianQuery(
    IStudentsDbContext dbContext,
    IDbConnectionFactory connectionFactory) : IGuardianQuery
{
    public Task<GuardianEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        return dbContext.Guardians
            .AsNoTracking()
            .SingleOrDefaultAsync(
                entity => entity.TenantId == tenantId && entity.GuardianId == id,
                cancellationToken);
    }

    public async Task<PagedResult<GuardianEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        const string countSql = """
            SELECT COUNT(*)
            FROM student.guardian
            WHERE tenant_id = @TenantId
              AND is_active = TRUE;
            """;

        const string pageSql = """
            SELECT
                tenant_id AS "TenantId",
                guardian_id AS "Id",
                user_id AS "UserId",
                full_name AS "FullName",
                cnic_number AS "CnicNumber",
                email AS "Email",
                phone AS "Phone"
            FROM student.guardian
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
            ORDER BY guardian_id
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
                cancellationToken: cancellationToken));

        var items = (await connection.QueryAsync<GuardianEntity>(
            new CommandDefinition(
                pageSql,
                parameters,
                cancellationToken: cancellationToken)))
            .AsList();

        return new PagedResult<GuardianEntity>(
            items,
            page,
            pageSize,
            totalCount);
    }

    public Task<bool> ExistsByCnicNumberAsync(
        Guid tenantId,
        string cnicNumber,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        return dbContext.Guardians
            .AsNoTracking()
            .AnyAsync(
                entity =>
                    entity.TenantId == tenantId && entity.CnicNumber == cnicNumber
                    && (!excludingId.HasValue || (excludingId.HasValue && entity.GuardianId != excludingId.Value)),
                cancellationToken);
    }
}
