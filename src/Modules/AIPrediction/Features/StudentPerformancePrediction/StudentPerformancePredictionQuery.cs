using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

/// <summary>
/// Executes database reads for <see cref="StudentPerformancePredictionEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class StudentPerformancePredictionQuery(
	IApplicationDbContext dbContext,
	IDbConnectionFactory connectionFactory) : IStudentPerformancePredictionQuery
{
	public Task<StudentPerformancePredictionEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<StudentPerformancePredictionEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.StudentPerformancePredictionId == id,
				cancellationToken);
	}

	public async Task<PagedResult<StudentPerformancePredictionEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM ai.student_performance_prediction
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				student_performance_prediction_id AS "Id"
			FROM ai.student_performance_prediction
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY student_performance_prediction_id
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

		var items = (await connection.QueryAsync<StudentPerformancePredictionEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<StudentPerformancePredictionEntity>(
			items,
			page,
			pageSize,
			totalCount);
	}

	public async Task<IReadOnlyCollection<StudentPerformancePredictionEntity>> GetByStudentIdAsync(
		Guid tenantId,
		Guid studentId,
		int limit,
		CancellationToken cancellationToken)
	{
		var pageSize = Math.Clamp(limit, 1, 100);

		return await dbContext
			.Set<StudentPerformancePredictionEntity>()
			.AsNoTracking()
			.Where(entity => entity.TenantId == tenantId && entity.StudentId == studentId)
			.OrderByDescending(entity => entity.GeneratedAt)
			.Take(pageSize)
			.ToArrayAsync(cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<StudentPerformancePredictionEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || (excludingId.HasValue && entity.StudentPerformancePredictionId != excludingId.Value)),
				cancellationToken);
	}
}
