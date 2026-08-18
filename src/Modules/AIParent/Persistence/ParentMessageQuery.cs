using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// EF-backed read persistence for ParentMessageEntity.
/// </summary>
public sealed class ParentMessageQuery(IEfMockStore store) : IParentMessageQuery
{
	public Task<ParentMessageEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ParentMessageEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ParentMessageEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ParentMessageEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ParentMessageEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
