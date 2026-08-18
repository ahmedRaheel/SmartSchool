using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// EF-backed read persistence for LearningResourceEntity.
/// </summary>
public sealed class LearningResourceQuery(IEfMockStore store) : ILearningResourceQuery
{
	public Task<LearningResourceEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<LearningResourceEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<LearningResourceEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<LearningResourceEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<LearningResourceEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
