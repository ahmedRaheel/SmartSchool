using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for PredictionEvaluationEntity.
/// </summary>
public sealed class PredictionEvaluationQuery(IEfMockStore store) : IPredictionEvaluationQuery
{
	public Task<PredictionEvaluationEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PredictionEvaluationEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PredictionEvaluationEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PredictionEvaluationEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PredictionEvaluationEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
