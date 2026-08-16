using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for PredictionModel.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PredictionModelCommand : IPredictionModelCommand
{
    public Task AddAsync(
        PredictionModel entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionModel create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PredictionModel entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionModel update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PredictionModel entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionModel delete persistence has not been connected to the module DbContext.");
    }
}
