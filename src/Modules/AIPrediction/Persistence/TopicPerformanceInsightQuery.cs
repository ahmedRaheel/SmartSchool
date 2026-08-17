using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Read-side persistence for TopicPerformanceInsightEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIPrediction module.
/// </summary>
public sealed class TopicPerformanceInsightQuery : ITopicPerformanceInsightQuery
{
    public Task<TopicPerformanceInsightEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsightEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<TopicPerformanceInsightEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsightEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsightEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
