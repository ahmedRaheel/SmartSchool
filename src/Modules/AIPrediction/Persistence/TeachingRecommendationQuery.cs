using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for TeachingRecommendationEntity.
/// </summary>
public sealed class TeachingRecommendationQuery(IEfMockStore store) : ITeachingRecommendationQuery
{
	public Task<TeachingRecommendationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TeachingRecommendationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TeachingRecommendationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TeachingRecommendationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TeachingRecommendationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
