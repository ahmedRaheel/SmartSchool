using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Features.UserProfile;

/// <summary>
/// Executes database reads for <see cref="UserProfileEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class UserProfileQuery(
    IApplicationDbContext dbContext,
    IDbConnectionFactory connectionFactory) : IUserProfileQuery
{
    public Task<UserProfileEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        return dbContext
            .Set<UserProfileEntity>()
            .AsNoTracking()
            .SingleOrDefaultAsync(
                entity => entity.TenantId == tenantId && entity.UserProfileId == id,
                cancellationToken);
    }

    public async Task<PagedResult<UserProfileEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        const string countSql = """
            SELECT COUNT(*)
            FROM public.UserProfile
            WHERE tenant_id = @TenantId
              AND is_active = TRUE;
            """;

        const string pageSql = """
            SELECT
                tenant_id AS "TenantId",
                userprofile_id AS "Id"
            FROM public.UserProfile
            WHERE tenant_id = @TenantId
              AND is_active = TRUE
            ORDER BY userprofile_id
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

        var items = (await connection.QueryAsync<UserProfileEntity>(
            new CommandDefinition(
                pageSql,
                parameters,
                cancellationToken: cancellationToken)))
            .AsList();

        return new PagedResult<UserProfileEntity>(
            items,
            page,
            pageSize,
            totalCount);
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        return dbContext
            .Set<UserProfileEntity>()
            .AsNoTracking()
            .AnyAsync(
                entity =>
                    entity.TenantId == tenantId
                    && EF.Property<string>(entity, "Code") == code
                    && (!excludingId.HasValue || (excludingId.HasValue && entity.UserProfileId != excludingId.Value)),
                cancellationToken);
    }
}
