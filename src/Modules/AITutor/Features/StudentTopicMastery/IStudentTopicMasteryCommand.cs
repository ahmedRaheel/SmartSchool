using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.StudentTopicMastery;

/// <summary>
/// Defines command persistence operations for StudentTopicMasteryEntity.
/// </summary>
public interface IStudentTopicMasteryCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        StudentTopicMasteryEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        StudentTopicMasteryEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        StudentTopicMasteryEntity entity,
        CancellationToken cancellationToken);
}
