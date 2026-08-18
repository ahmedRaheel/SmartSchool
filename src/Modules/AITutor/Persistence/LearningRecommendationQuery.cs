using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// EF-backed read persistence for LearningRecommendationEntity.
/// </summary>
public sealed class LearningRecommendationQuery(IEfMockStore store) : ILearningRecommendationQuery
{
	public Task<LearningRecommendationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<LearningRecommendationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<LearningRecommendationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<LearningRecommendationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<LearningRecommendationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
