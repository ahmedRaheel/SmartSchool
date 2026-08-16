using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for PredictionEvaluation.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PredictionEvaluationCommand : IPredictionEvaluationCommand
{
    public Task AddAsync(
        PredictionEvaluation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluation create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PredictionEvaluation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluation update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PredictionEvaluation entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluation delete persistence has not been connected to the module DbContext.");
    }
}
