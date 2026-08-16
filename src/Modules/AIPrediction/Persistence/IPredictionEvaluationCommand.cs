using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface IPredictionEvaluationCommand
{
    Task AddAsync(
        PredictionEvaluation entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        PredictionEvaluation entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        PredictionEvaluation entity,
        CancellationToken cancellationToken);
}
