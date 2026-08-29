using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database reads for <see cref="AcademicYearEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class AcademicYearQuery(IDbConnectionFactory connectionFactory) : IAcademicYearQuery
{
	public async Task<AcademicYearEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT *
			FROM academic.academic_year
			WHERE tenant_id = @TenantId
			  AND start_date = @Id
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.QuerySingleOrDefaultAsync<AcademicYearEntity>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, Id = id },
				cancellationToken: cancellationToken)).ConfigureAwait(false).ConfigureAwait(false);
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
				cancellationToken: cancellationToken)).ConfigureAwait(false);

		var items = (await connection.QueryAsync<AcademicYearEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken))).ConfigureAwait(false)
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
				cancellationToken: cancellationToken)).ConfigureAwait(false);
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
				FROM academic.academic_year
				WHERE tenant_id = @TenantId
				  AND code = @Code
				  AND (@ExcludingId IS NULL OR start_date <> @ExcludingId)
			);
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.ExecuteScalarAsync<bool>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, Code = code, ExcludingId = excludingId },
				cancellationToken: cancellationToken)).ConfigureAwait(false).ConfigureAwait(false);
	}
}
