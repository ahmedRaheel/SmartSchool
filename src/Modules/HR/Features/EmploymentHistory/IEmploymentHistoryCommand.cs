using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.EmploymentHistory;

/// <summary>
/// Defines command persistence operations for EmploymentHistoryEntity.
/// </summary>
public interface IEmploymentHistoryCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        EmploymentHistoryEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        EmploymentHistoryEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        EmploymentHistoryEntity entity,
        CancellationToken cancellationToken);
}
