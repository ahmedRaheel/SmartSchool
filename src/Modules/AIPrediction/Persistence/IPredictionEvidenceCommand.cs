using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface IPredictionEvidenceCommand
{
    Task AddAsync(
        PredictionEvidence entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        PredictionEvidence entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        PredictionEvidence entity,
        CancellationToken cancellationToken);
}
