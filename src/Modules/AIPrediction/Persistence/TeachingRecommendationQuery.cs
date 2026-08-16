using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Read-side persistence for TeachingRecommendation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AIPrediction module.
/// </summary>
public sealed class TeachingRecommendationQuery : ITeachingRecommendationQuery
{
    public Task<TeachingRecommendation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<TeachingRecommendation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TeachingRecommendation uniqueness persistence has not been connected to the module DbContext.");
    }
}
