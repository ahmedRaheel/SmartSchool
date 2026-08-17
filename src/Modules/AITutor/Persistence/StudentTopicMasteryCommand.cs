using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for StudentTopicMasteryEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class StudentTopicMasteryCommand : IStudentTopicMasteryCommand
{
    public Task AddAsync(
        StudentTopicMasteryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMasteryEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        StudentTopicMasteryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMasteryEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        StudentTopicMasteryEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "StudentTopicMasteryEntity delete persistence has not been connected to the module DbContext.");
    }
}
