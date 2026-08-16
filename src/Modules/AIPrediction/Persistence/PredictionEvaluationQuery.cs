using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Read-side persistence for PredictionEvaluation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIPrediction module.
/// </summary>
public sealed class PredictionEvaluationQuery : IPredictionEvaluationQuery
{
    public Task<PredictionEvaluation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<PredictionEvaluation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvaluation uniqueness persistence has not been connected to the module DbContext.");
    }
}
