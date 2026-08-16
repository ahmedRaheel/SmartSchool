using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Read-side persistence for Discount.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Finance module.
/// </summary>
public sealed class DiscountQuery : IDiscountQuery
{
    public Task<Discount?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Discount read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Discount>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Discount paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Discount uniqueness persistence has not been connected to the module DbContext.");
    }
}
