using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for TutorSession.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TutorSessionCommand : ITutorSessionCommand
{
    public Task AddAsync(
        TutorSession entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSession create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TutorSession entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSession update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TutorSession entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorSession delete persistence has not been connected to the module DbContext.");
    }
}
