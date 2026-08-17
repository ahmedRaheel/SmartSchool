using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for QuizAttemptEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class QuizAttemptCommand : IQuizAttemptCommand
{
    public Task AddAsync(
        QuizAttemptEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "QuizAttemptEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        QuizAttemptEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "QuizAttemptEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        QuizAttemptEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "QuizAttemptEntity delete persistence has not been connected to the module DbContext.");
    }
}
