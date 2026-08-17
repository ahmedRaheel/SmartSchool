using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for GeneratedQuizEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GeneratedQuizCommand : IGeneratedQuizCommand
{
    public Task AddAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedQuizEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedQuizEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedQuizEntity delete persistence has not been connected to the module DbContext.");
    }
}
