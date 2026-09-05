using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.ClassPerformanceInsight;

/// <summary>
/// Defines query persistence operations for ClassPerformanceInsightEntity.
/// </summary>
public interface IClassPerformanceInsightQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<ClassPerformanceInsightEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<ClassPerformanceInsightEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
