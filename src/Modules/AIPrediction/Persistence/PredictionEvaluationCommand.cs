using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed write persistence for PredictionEvaluationEntity.
/// </summary>
public sealed class PredictionEvaluationCommand(IEfMockStore store) : IPredictionEvaluationCommand
{
	public Task AddAsync(PredictionEvaluationEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(PredictionEvaluationEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(PredictionEvaluationEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
