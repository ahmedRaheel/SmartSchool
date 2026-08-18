using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed read persistence for KnowledgeChunkEntity.
/// </summary>
public sealed class KnowledgeChunkQuery(IEfMockStore store) : IKnowledgeChunkQuery
{
	public Task<KnowledgeChunkEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<KnowledgeChunkEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<KnowledgeChunkEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<KnowledgeChunkEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<KnowledgeChunkEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
