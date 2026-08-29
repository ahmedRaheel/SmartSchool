using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database reads for <see cref="AcademicYearEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class AcademicYearQuery(
	IApplicationDbContext dbContext,
	IDbConnectionFactory connectionFactory) : IAcademicYearQuery
{
	public Task<AcademicYearEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<AcademicYearEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.AcademicYearId == id,
				cancellationToken);
	}

	public async Task<PagedResult<AcademicYearEntity>> GetPageAsync(
		Guid tenantId,
		Guid campusId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM academic.academic_year
			WHERE tenant_id = @TenantId
			  AND campus_id = @CampusId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				academic_year_id AS "AcademicYearId",
				tenant_id AS "TenantId",
				campus_id AS "CampusId",
				start_date AS "StartDate",
				end_date AS "EndDate",
				is_current AS "IsCurrent",
				code AS "Code",
				name AS "Name",
				metadata_json::text AS "MetadataJson",
				is_active AS "IsActive",
				created_at AS "CreatedAt",
				updated_at AS "UpdatedAt",
				row_version AS "RowVersion"
			FROM academic.academic_year
			WHERE tenant_id = @TenantId
			  AND campus_id = @CampusId
			  AND is_active = TRUE
			ORDER BY start_date DESC, academic_year_id
			LIMIT @PageSize OFFSET @Offset;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken);

		var parameters = new
		{
			TenantId = tenantId,
			CampusId = campusId,
			PageSize = pageSize,
			Offset = (page - 1) * pageSize
		};

		var totalCount = await connection.ExecuteScalarAsync<long>(
			new CommandDefinition(
				countSql,
				parameters,
				cancellationToken: cancellationToken));

		var items = (await connection.QueryAsync<AcademicYearEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<AcademicYearEntity>(
			items,
			page,
			pageSize,
			totalCount);
	}

	public async Task<bool> CampusExistsAsync(
		Guid tenantId,
		Guid campusId,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT EXISTS (
				SELECT 1
				FROM org.campus
				WHERE tenant_id = @TenantId
				  AND campus_id = @CampusId
				  AND is_active = TRUE
			);
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken);

		return await connection.ExecuteScalarAsync<bool>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, CampusId = campusId },
				cancellationToken: cancellationToken));
	}

	public Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<AcademicYearEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || (excludingId.HasValue && entity.AcademicYearId != excludingId.Value)),
				cancellationToken);
	}
}
