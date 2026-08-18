using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed read persistence for TutorConversationEntity.
/// </summary>
public sealed class TutorConversationQuery(IEfMockStore store) : ITutorConversationQuery
{
	public Task<TutorConversationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TutorConversationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TutorConversationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TutorConversationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TutorConversationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
