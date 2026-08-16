using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for PredictionEvidence.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PredictionEvidenceCommand : IPredictionEvidenceCommand
{
    public Task AddAsync(
        PredictionEvidence entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidence create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PredictionEvidence entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidence update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PredictionEvidence entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidence delete persistence has not been connected to the module DbContext.");
    }
}
