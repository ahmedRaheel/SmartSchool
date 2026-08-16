using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for GeneratedQuiz.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class GeneratedQuizCommand : IGeneratedQuizCommand
{
    public Task AddAsync(
        GeneratedQuiz entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedQuiz create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        GeneratedQuiz entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedQuiz update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        GeneratedQuiz entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedQuiz delete persistence has not been connected to the module DbContext.");
    }
}
