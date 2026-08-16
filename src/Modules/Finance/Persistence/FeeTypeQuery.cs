using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Read-side persistence for FeeType.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Finance module.
/// </summary>
public sealed class FeeTypeQuery : IFeeTypeQuery
{
    public Task<FeeType?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeType read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<FeeType>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeType paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeType uniqueness persistence has not been connected to the module DbContext.");
    }
}
