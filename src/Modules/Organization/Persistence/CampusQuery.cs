using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Executes database reads for <see cref="CampusEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class CampusQuery(
	IApplicationDbContext dbContext,
	IDbConnectionFactory connectionFactory) : ICampusQuery
{
	public Task<CampusEntity?> GetByIdAsync(
		Guid? tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<CampusEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => (!tenantId.HasValue || entity.TenantId == tenantId.Value) && entity.CampusId == id,
				cancellationToken);
	}

	public async Task<PagedResult<CampusEntity>> GetPageAsync(
		Guid? tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM org."Campus"
			WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				campus_id AS "Id"
			FROM org."Campus"
			WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
			  AND is_active = TRUE
			ORDER BY campus_id
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

		var items = (await connection.QueryAsync<CampusEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<CampusEntity>(
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
			.Set<CampusEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || (excludingId.HasValue && entity.CampusId != excludingId.Value)),
				cancellationToken);
	}
}
