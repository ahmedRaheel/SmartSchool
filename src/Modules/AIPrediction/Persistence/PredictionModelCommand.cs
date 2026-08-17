using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for PredictionModelEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PredictionModelCommand : IPredictionModelCommand
{
    public Task AddAsync(
        PredictionModelEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionModelEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PredictionModelEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionModelEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PredictionModelEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionModelEntity delete persistence has not been connected to the module DbContext.");
    }
}
