using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Read-side persistence for TenantEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Tenancy module.
/// </summary>
public sealed class TenantQuery : ITenantQuery
{
    public Task<TenantEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TenantEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<TenantEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TenantEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TenantEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
