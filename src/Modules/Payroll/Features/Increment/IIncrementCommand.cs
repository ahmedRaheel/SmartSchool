using System.Threading.Tasks;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Features.Increment;

/// <summary>
/// Defines command persistence operations for IncrementEntity.
/// </summary>
public interface IIncrementCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        IncrementEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        IncrementEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        IncrementEntity entity,
        CancellationToken cancellationToken);
}
