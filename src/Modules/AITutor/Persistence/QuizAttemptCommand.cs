using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Write-side persistence for QuizAttempt.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class QuizAttemptCommand : IQuizAttemptCommand
{
    public Task AddAsync(
        QuizAttempt entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "QuizAttempt create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        QuizAttempt entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "QuizAttempt update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        QuizAttempt entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "QuizAttempt delete persistence has not been connected to the module DbContext.");
    }
}
