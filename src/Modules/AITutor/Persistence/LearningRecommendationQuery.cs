using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Executes database reads for <see cref="LearningRecommendationEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class LearningRecommendationQuery(IDbConnectionFactory connectionFactory) : ILearningRecommendationQuery
{
	public async Task<LearningRecommendationEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT *
			FROM ai_tutor.learning_recommendation
			WHERE tenant_id = @TenantId
			  AND learning_recommendation_id = @Id
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.QuerySingleOrDefaultAsync<LearningRecommendationEntity>(
			new CommandDefinition(
				sql,
				new
				{
					TenantId = tenantId,
					Id = id
				},
				cancellationToken: cancellationToken)).ConfigureAwait(false);
	}

	public async Task<PagedResult<LearningRecommendationEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM ai_tutor.learning_recommendation
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				learning_recommendation_id AS "Id"
			FROM ai_tutor.learning_recommendation
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY learning_recommendation_id
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

		var items = (await connection.QueryAsync<LearningRecommendationEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)).ConfigureAwait(false))
			.AsList();

		return new PagedResult<LearningRecommendationEntity>(
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
				FROM ai_tutor.learning_recommendation
				WHERE tenant_id = @TenantId
				  AND code = @Code
				  AND (@ExcludingId IS NULL OR learning_recommendation_id <> @ExcludingId)
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
