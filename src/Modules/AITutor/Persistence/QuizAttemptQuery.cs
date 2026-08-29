using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Executes database reads for <see cref="QuizAttemptEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class QuizAttemptQuery(IDbConnectionFactory connectionFactory) : IQuizAttemptQuery
{
	public async Task<QuizAttemptEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT *
			FROM ai_tutor.student_quiz_attempt
			WHERE tenant_id = @TenantId
			  AND student_quiz_attempt_id = @Id
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.QuerySingleOrDefaultAsync<QuizAttemptEntity>(
			new CommandDefinition(
				sql,
				new
				{
					TenantId = tenantId,
					Id = id
				},
				cancellationToken: cancellationToken)).ConfigureAwait(false);
	}

	public async Task<PagedResult<QuizAttemptEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM ai_tutor.student_quiz_attempt
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				student_quiz_attempt_id AS "Id"
			FROM ai_tutor.student_quiz_attempt
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY student_quiz_attempt_id
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

		var items = (await connection.QueryAsync<QuizAttemptEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)).ConfigureAwait(false))
			.AsList();

		return new PagedResult<QuizAttemptEntity>(
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
				FROM ai_tutor.student_quiz_attempt
				WHERE tenant_id = @TenantId
				  AND code = @Code
				  AND (@ExcludingId IS NULL OR student_quiz_attempt_id <> @ExcludingId)
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
