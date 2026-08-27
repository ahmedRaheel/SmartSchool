using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Executes database reads for <see cref="SchoolEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class SchoolQuery(
	IApplicationDbContext dbContext,
	IDbConnectionFactory connectionFactory) : ISchoolQuery
{
	public Task<SchoolEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<SchoolEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.SchoolId == id,
				cancellationToken);
	}

	public async Task<PagedResult<SchoolEntity>> GetPageAsync(
		Guid? tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM org.school
			WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
			AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT 
				school_id AS "SchoolId", tenant_id AS "TenantId", code AS "Code", name AS "Name",
				registration_number AS "RegistrationNumber", email AS "Email", phone AS "Phone", fax AS "Fax", website AS "Website",
				address AS "Address", city AS "City", province AS "Province", country AS "Country", logo_url AS "LogoUrl",
				is_active AS "IsActive", created_at AS "CreatedAt", updated_at AS "UpdatedAt", row_version AS "RowVersion"
			FROM org.school
			WHERE (@TenantId IS NULL OR tenant_id = @TenantId)
				AND is_active = TRUE
			ORDER BY school_id
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

		var items = (await connection.QueryAsync<SchoolEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<SchoolEntity>(
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
			.Set<SchoolEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || (excludingId.HasValue && entity.SchoolId != excludingId.Value)),
				cancellationToken);
	}
}
