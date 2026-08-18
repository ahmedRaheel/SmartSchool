using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for PredictionModelEntity.
/// </summary>
public sealed class PredictionModelQuery(IEfMockStore store) : IPredictionModelQuery
{
	public Task<PredictionModelEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PredictionModelEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PredictionModelEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PredictionModelEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PredictionModelEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
