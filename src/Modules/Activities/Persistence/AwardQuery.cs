using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Read-side persistence for Award.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Activities module.
/// </summary>
public sealed class AwardQuery : IAwardQuery
{
    public Task<Award?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Award read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Award>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Award paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Award uniqueness persistence has not been connected to the module DbContext.");
    }
}
