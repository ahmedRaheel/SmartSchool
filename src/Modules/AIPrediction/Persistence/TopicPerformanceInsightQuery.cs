using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for TopicPerformanceInsightEntity.
/// </summary>
public sealed class TopicPerformanceInsightQuery(IEfMockStore store) : ITopicPerformanceInsightQuery
{
	public Task<TopicPerformanceInsightEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<TopicPerformanceInsightEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<TopicPerformanceInsightEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<TopicPerformanceInsightEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<TopicPerformanceInsightEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
