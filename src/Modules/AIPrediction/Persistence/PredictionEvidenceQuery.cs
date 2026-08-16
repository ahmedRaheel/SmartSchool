using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Read-side persistence for PredictionEvidence.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIPrediction module.
/// </summary>
public sealed class PredictionEvidenceQuery : IPredictionEvidenceQuery
{
    public Task<PredictionEvidence?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidence read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<PredictionEvidence>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidence paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PredictionEvidence uniqueness persistence has not been connected to the module DbContext.");
    }
}
