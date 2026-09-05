using System.Threading.Tasks;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Features.Lesson;

/// <summary>
/// Defines command persistence operations for LessonEntity.
/// </summary>
public interface ILessonCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        LessonEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        LessonEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        LessonEntity entity,
        CancellationToken cancellationToken);
}
