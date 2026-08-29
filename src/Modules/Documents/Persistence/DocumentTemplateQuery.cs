using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Executes database reads for <see cref="DocumentTemplateEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class DocumentTemplateQuery(IDbConnectionFactory connectionFactory) : IDocumentTemplateQuery
{
	public async Task<DocumentTemplateEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT *
			FROM document.document_template
			WHERE tenant_id = @TenantId
			  AND document_template_id = @Id
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.QuerySingleOrDefaultAsync<DocumentTemplateEntity>(
			new CommandDefinition(
				sql,
				new
				{
					TenantId = tenantId,
					Id = id
				},
				cancellationToken: cancellationToken)).ConfigureAwait(false);
	}

	public async Task<PagedResult<DocumentTemplateEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM document.document_template
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				document_template_id AS "Id"
			FROM document.document_template
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY document_template_id
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

		var items = (await connection.QueryAsync<DocumentTemplateEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)).ConfigureAwait(false))
			.AsList();

		return new PagedResult<DocumentTemplateEntity>(
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
				FROM document.document_template
				WHERE tenant_id = @TenantId
				  AND code = @Code
				  AND (@ExcludingId IS NULL OR document_template_id <> @ExcludingId)
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
