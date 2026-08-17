using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for PredictionEvaluationEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PredictionEvaluationCommand : IPredictionEvaluationCommand
{
    public Task AddAsync(
        PredictionEvaluationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluationEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PredictionEvaluationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluationEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PredictionEvaluationEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluationEntity delete persistence has not been connected to the module DbContext.");
    }
}
