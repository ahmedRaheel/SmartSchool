using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed write persistence for PredictionEvidenceEntity.
/// </summary>
public sealed class PredictionEvidenceCommand(IEfMockStore store) : IPredictionEvidenceCommand
{
	public Task AddAsync(PredictionEvidenceEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(PredictionEvidenceEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(PredictionEvidenceEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
