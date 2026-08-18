using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// EF-backed read persistence for ConversationParticipantEntity.
/// </summary>
public sealed class ConversationParticipantQuery(IEfMockStore store) : IConversationParticipantQuery
{
	public Task<ConversationParticipantEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ConversationParticipantEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ConversationParticipantEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ConversationParticipantEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ConversationParticipantEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
