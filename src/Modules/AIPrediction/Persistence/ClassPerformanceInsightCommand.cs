using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for ClassPerformanceInsight.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class ClassPerformanceInsightCommand : IClassPerformanceInsightCommand
{
    public Task AddAsync(
        ClassPerformanceInsight entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsight create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        ClassPerformanceInsight entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsight update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        ClassPerformanceInsight entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsight delete persistence has not been connected to the module DbContext.");
    }
}
