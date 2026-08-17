using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for ClassPerformanceInsightEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ClassPerformanceInsightCommand : IClassPerformanceInsightCommand
{
    public Task AddAsync(
        ClassPerformanceInsightEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsightEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ClassPerformanceInsightEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsightEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ClassPerformanceInsightEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsightEntity delete persistence has not been connected to the module DbContext.");
    }
}
