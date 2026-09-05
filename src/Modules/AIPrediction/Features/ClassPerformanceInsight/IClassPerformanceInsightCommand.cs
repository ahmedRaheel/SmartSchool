using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.ClassPerformanceInsight;

/// <summary>
/// Defines command persistence operations for ClassPerformanceInsightEntity.
/// </summary>
public interface IClassPerformanceInsightCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        ClassPerformanceInsightEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        ClassPerformanceInsightEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        ClassPerformanceInsightEntity entity,
        CancellationToken cancellationToken);
}
