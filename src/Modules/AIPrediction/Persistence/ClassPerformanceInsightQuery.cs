using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for ClassPerformanceInsightEntity.
/// </summary>
public sealed class ClassPerformanceInsightQuery(IEfMockStore store) : IClassPerformanceInsightQuery
{
	public Task<ClassPerformanceInsightEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<ClassPerformanceInsightEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<ClassPerformanceInsightEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<ClassPerformanceInsightEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<ClassPerformanceInsightEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
