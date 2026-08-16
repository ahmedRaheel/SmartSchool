using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface ITopicPerformanceInsightQuery
{
    Task<TopicPerformanceInsight?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<TopicPerformanceInsight>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
