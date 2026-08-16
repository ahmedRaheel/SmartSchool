using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

public interface ILearningRecommendationQuery
{
    Task<LearningRecommendation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<LearningRecommendation>> GetPageAsync(
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
