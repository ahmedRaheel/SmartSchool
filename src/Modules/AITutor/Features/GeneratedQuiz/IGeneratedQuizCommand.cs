using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.GeneratedQuiz;

/// <summary>
/// Defines command persistence operations for GeneratedQuizEntity.
/// </summary>
public interface IGeneratedQuizCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        GeneratedQuizEntity entity,
        CancellationToken cancellationToken);
}
