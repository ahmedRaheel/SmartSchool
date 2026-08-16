using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for StudentIntervention.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentInterventionCommand : IStudentInterventionCommand
{
    public Task AddAsync(
        StudentIntervention entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentIntervention create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentIntervention entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentIntervention update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentIntervention entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentIntervention delete persistence has not been connected to the module DbContext.");
    }
}
