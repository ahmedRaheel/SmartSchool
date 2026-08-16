using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface IGeneratedQuizCommand
{
    Task AddAsync(
        GeneratedQuiz entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        GeneratedQuiz entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        GeneratedQuiz entity,
        CancellationToken cancellationToken);
}
