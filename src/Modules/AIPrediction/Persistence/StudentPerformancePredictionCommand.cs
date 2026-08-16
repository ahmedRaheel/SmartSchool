using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for StudentPerformancePrediction.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentPerformancePredictionCommand : IStudentPerformancePredictionCommand
{
    public Task AddAsync(
        StudentPerformancePrediction entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePrediction create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentPerformancePrediction entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePrediction update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentPerformancePrediction entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePrediction delete persistence has not been connected to the module DbContext.");
    }
}
