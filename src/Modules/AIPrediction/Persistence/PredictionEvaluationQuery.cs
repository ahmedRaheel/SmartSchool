using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database reads for <see cref="PredictionEvaluationEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class PredictionEvaluationQuery(
	IApplicationDbContext dbContext,
	IDbConnectionFactory connectionFactory) : IPredictionEvaluationQuery
{
	public Task<PredictionEvaluationEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		return dbContext
			.Set<PredictionEvaluationEntity>()
			.AsNoTracking()
			.SingleOrDefaultAsync(
				entity => entity.TenantId == tenantId && entity.Id == id,
				cancellationToken);
	}

	public async Task<PagedResult<PredictionEvaluationEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM ai.prediction_evaluation
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				predictionevaluation_id AS "Id"
			FROM ai.prediction_evaluation
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY predictionevaluation_id
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

		var items = (await connection.QueryAsync<PredictionEvaluationEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)))
			.AsList();

		return new PagedResult<PredictionEvaluationEntity>(
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
			.Set<PredictionEvaluationEntity>()
			.AsNoTracking()
			.AnyAsync(
				entity =>
					entity.TenantId == tenantId
					&& EF.Property<string>(entity, "Code") == code
					&& (!excludingId.HasValue || entity.Id != excludingId.Value),
				cancellationToken);
	}
}
