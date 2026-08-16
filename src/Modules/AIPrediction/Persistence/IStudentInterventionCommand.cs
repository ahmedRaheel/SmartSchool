using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface IStudentInterventionCommand
{
    Task AddAsync(
        StudentIntervention entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        StudentIntervention entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        StudentIntervention entity,
        CancellationToken cancellationToken);
}
