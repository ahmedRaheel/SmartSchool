using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface IPredictionModelCommand
{
    Task AddAsync(
        PredictionModel entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        PredictionModel entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        PredictionModel entity,
        CancellationToken cancellationToken);
}
