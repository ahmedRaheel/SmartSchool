using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for TutorSessionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TutorSessionCommand : ITutorSessionCommand
{
    public Task AddAsync(
        TutorSessionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSessionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TutorSessionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSessionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TutorSessionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSessionEntity delete persistence has not been connected to the module DbContext.");
    }
}
