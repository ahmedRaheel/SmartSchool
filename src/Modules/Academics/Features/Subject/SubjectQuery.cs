using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Features.Subject;

/// <summary>
/// Executes database reads for <see cref="SubjectEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class SubjectQuery(IDbConnectionFactory connectionFactory) : ISubjectQuery
{
	public async Task<SubjectEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT *
			FROM academic.subject
			WHERE tenant_id = @TenantId
			  AND subject_id = @Id
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.QuerySingleOrDefaultAsync<SubjectEntity>(
			new CommandDefinition(
				sql,
				new
				{
					TenantId = tenantId,
					Id = id
				},
				cancellationToken: cancellationToken)).ConfigureAwait(false);
	}

	public async Task<PagedResult<SubjectEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM academic.subject
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				subject_id AS "Id"
			FROM academic.subject
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY subject_id
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

		var items = (await connection.QueryAsync<SubjectEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)).ConfigureAwait(false))
			.AsList();

		return new PagedResult<SubjectEntity>(
			items,
			page,
			pageSize,
			totalCount);
	}

	public async Task<string?> GetBranchCodeAsync(
		Guid tenantId,
		Guid branchId,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT code
			FROM org.campus
			WHERE tenant_id = @TenantId
			  AND campus_id = @BranchId
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken);

		return await connection.ExecuteScalarAsync<string?>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, BranchId = branchId },
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
				FROM academic.subject
				WHERE tenant_id = @TenantId
				  AND code = @Code
				  AND (@ExcludingId IS NULL OR subject_id <> @ExcludingId)
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
