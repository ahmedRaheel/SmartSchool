using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;

/// <summary>
/// Defines query persistence operations for PredictionEvaluationEntity.
/// </summary>
public interface IPredictionEvaluationQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PredictionEvaluationEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<PredictionEvaluationEntity>> GetPageAsync(
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
