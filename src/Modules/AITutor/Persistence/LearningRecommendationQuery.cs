using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Read-side persistence for LearningRecommendation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AITutor module.
/// </summary>
public sealed class LearningRecommendationQuery : ILearningRecommendationQuery
{
    public Task<LearningRecommendation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<LearningRecommendation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningRecommendation uniqueness persistence has not been connected to the module DbContext.");
    }
}
