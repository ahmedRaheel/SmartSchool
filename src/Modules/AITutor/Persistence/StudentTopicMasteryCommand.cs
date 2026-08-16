using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for StudentTopicMastery.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentTopicMasteryCommand : IStudentTopicMasteryCommand
{
    public Task AddAsync(
        StudentTopicMastery entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMastery create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentTopicMastery entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMastery update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentTopicMastery entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMastery delete persistence has not been connected to the module DbContext.");
    }
}
