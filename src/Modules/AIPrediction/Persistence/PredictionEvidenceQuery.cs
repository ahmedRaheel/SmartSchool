using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed read persistence for PredictionEvidenceEntity.
/// </summary>
public sealed class PredictionEvidenceQuery(IEfMockStore store) : IPredictionEvidenceQuery
{
	public Task<PredictionEvidenceEntity?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken)
	{
		return store.GetByIdAsync<PredictionEvidenceEntity>(tenantId, id, cancellationToken);
	}

	public Task<PagedResult<PredictionEvidenceEntity>> GetPageAsync(Guid tenantId, int page, int pageSize, CancellationToken cancellationToken)
	{
		return store.GetPageAsync<PredictionEvidenceEntity>(tenantId, page, pageSize, cancellationToken);
	}

	public Task<bool> ExistsByCodeAsync(Guid tenantId, string code, Guid? excludingId, CancellationToken cancellationToken)
	{
		return store.ExistsByCodeAsync<PredictionEvidenceEntity>(tenantId, code, excludingId, cancellationToken);
	}

}
