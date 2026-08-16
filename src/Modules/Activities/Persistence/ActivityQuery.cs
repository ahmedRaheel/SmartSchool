using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Read-side persistence for Activity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Activities module.
/// </summary>
public sealed class ActivityQuery : IActivityQuery
{
    public Task<Activity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Activity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Activity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Activity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Activity uniqueness persistence has not been connected to the module DbContext.");
    }
}
