using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed read persistence for KnowledgeCollectionEntity.
/// </summary>
public sealed class KnowledgeCollectionQuery(IEfMockStore store) : IKnowledgeCollectionQuery
{
	public Task<KnowledgeCollectionEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<KnowledgeCollectionEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<KnowledgeCollectionEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<KnowledgeCollectionEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<KnowledgeCollectionEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
