using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Read-side persistence for ClassPerformanceInsightEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIPrediction module.
/// </summary>
public sealed class ClassPerformanceInsightQuery : IClassPerformanceInsightQuery
{
    public Task<ClassPerformanceInsightEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsightEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ClassPerformanceInsightEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsightEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ClassPerformanceInsightEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
