using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Interview;

/// <summary>
/// Defines command persistence operations for InterviewEntity.
/// </summary>
public interface IInterviewCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        InterviewEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        InterviewEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        InterviewEntity entity,
        CancellationToken cancellationToken);
}
