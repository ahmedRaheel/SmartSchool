using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for PredictionEvidenceEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PredictionEvidenceCommand : IPredictionEvidenceCommand
{
    public Task AddAsync(
        PredictionEvidenceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidenceEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PredictionEvidenceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidenceEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PredictionEvidenceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidenceEntity delete persistence has not been connected to the module DbContext.");
    }
}
