using Dapper;
using System.Threading.Tasks;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;
using SmartSchool.SharedKernel;

using SmartSchool.Modules.AIInquiry.Features.InquiryConversation;

namespace SmartSchool.Modules.AIInquiry.Features.DataAccess.nquiryConversation;

/// <summary>
/// Executes database reads for <see cref="InquiryConversationEntity"/>.
/// Read operations are tenant-scoped and use no-tracking queries.
/// </summary>
public sealed class InquiryConversationQuery(IDbConnectionFactory connectionFactory) : IInquiryConversationQuery
{
	public async Task<InquiryConversationEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken)
	{
		const string sql = """
			SELECT *
			FROM ai_core.inquiry_conversation
			WHERE tenant_id = @TenantId
			  AND inquiry_conversation_id = @Id
			  AND is_active = TRUE;
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.QuerySingleOrDefaultAsync<InquiryConversationEntity>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, Id = id },
				cancellationToken: cancellationToken)).ConfigureAwait(false);
	}

	public async Task<PagedResult<InquiryConversationEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken)
	{
		const string countSql = """
			SELECT COUNT(*)
			FROM ai_core.inquiry_conversation
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE;
			""";

		const string pageSql = """
			SELECT
				tenant_id AS "TenantId",
				inquiry_conversation_id AS "Id"
			FROM ai_core.inquiry_conversation
			WHERE tenant_id = @TenantId
			  AND is_active = TRUE
			ORDER BY inquiry_conversation_id
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

		var items = (await connection.QueryAsync<InquiryConversationEntity>(
			new CommandDefinition(
				pageSql,
				parameters,
				cancellationToken: cancellationToken)).ConfigureAwait(false))
			.AsList();

		return new PagedResult<InquiryConversationEntity>(
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
				FROM ai_core.inquiry_conversation
				WHERE tenant_id = @TenantId
				  AND code = @Code
				  AND (@ExcludingId IS NULL OR inquiry_conversation_id <> @ExcludingId)
			);
			""";

		await using var connection =
			await connectionFactory.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

		return await connection.ExecuteScalarAsync<bool>(
			new CommandDefinition(
				sql,
				new { TenantId = tenantId, Code = code, ExcludingId = excludingId },
				cancellationToken: cancellationToken)).ConfigureAwait(false);
	}
}
