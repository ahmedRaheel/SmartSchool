using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface IQuizAttemptCommand
{
    Task AddAsync(
        QuizAttempt entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        QuizAttempt entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        QuizAttempt entity,
        CancellationToken cancellationToken);
}
