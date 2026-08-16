using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Read-side persistence for TopicPerformanceInsight.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIPrediction module.
/// </summary>
public sealed class TopicPerformanceInsightQuery : ITopicPerformanceInsightQuery
{
    public Task<TopicPerformanceInsight?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsight read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<TopicPerformanceInsight>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsight paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsight uniqueness persistence has not been connected to the module DbContext.");
    }
}
