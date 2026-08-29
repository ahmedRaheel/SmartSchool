using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database reads for <see cref="PredictionEvidenceEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class PredictionEvidenceQuery(IDbConnectionFactory connectionFactory) : IPredictionEvidenceQuery
{
	public async Task<PredictionEvidenceEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT *
			FROM ai.prediction_evidence
			WHERE tenant_id = @TenantId
			  AND prediction_evidence_id = @Id
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.QuerySingleOrDefaultAsync<PredictionEvidenceEntity>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, Id = id },
				cancellationToken: cancellationToken)).ConfigureAwait(false).ConfigureAwait(false);
	}

	public async Task<PagedResult<PredictionEvidenceEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM ai.prediction_evidence
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				prediction_evidence_id AS "Id"
			FROM ai.prediction_evidence
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY prediction_evidence_id
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

		var items = (await connection.QueryAsync<PredictionEvidenceEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken))).ConfigureAwait(false)
			.AsList();

		return new PagedResult<PredictionEvidenceEntity>(
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
				FROM ai.prediction_evidence
				WHERE tenant_id = @TenantId
				  AND code = @Code
				  AND (@ExcludingId IS NULL OR prediction_evidence_id <> @ExcludingId)
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
