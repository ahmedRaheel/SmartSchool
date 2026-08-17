using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for StudentInterventionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentInterventionCommand : IStudentInterventionCommand
{
    public Task AddAsync(
        StudentInterventionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentInterventionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentInterventionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentInterventionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentInterventionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentInterventionEntity delete persistence has not been connected to the module DbContext.");
    }
}
