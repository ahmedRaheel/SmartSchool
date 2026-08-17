using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for StudentPerformancePredictionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentPerformancePredictionCommand : IStudentPerformancePredictionCommand
{
    public Task AddAsync(
        StudentPerformancePredictionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePredictionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentPerformancePredictionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePredictionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentPerformancePredictionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentPerformancePredictionEntity delete persistence has not been connected to the module DbContext.");
    }
}
