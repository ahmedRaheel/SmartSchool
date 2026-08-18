using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// EF-backed read persistence for ConversationEntity.
/// </summary>
public sealed class ConversationQuery(IEfMockStore store) : IConversationQuery
{
	public Task<ConversationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ConversationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ConversationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ConversationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ConversationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
