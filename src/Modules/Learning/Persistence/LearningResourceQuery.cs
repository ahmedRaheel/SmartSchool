using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Read-side persistence for LearningResourceEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Learning module.
/// </summary>
public sealed class LearningResourceQuery : ILearningResourceQuery
{
    public Task<LearningResourceEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResourceEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<LearningResourceEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResourceEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "LearningResourceEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
