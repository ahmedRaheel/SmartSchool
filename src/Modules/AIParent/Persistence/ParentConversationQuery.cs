using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// EF-backed read persistence for ParentConversationEntity.
/// </summary>
public sealed class ParentConversationQuery(IEfMockStore store) : IParentConversationQuery
{
	public Task<ParentConversationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ParentConversationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ParentConversationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ParentConversationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ParentConversationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
