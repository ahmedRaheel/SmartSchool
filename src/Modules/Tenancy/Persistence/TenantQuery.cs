using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Read-side persistence for Tenant.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Tenancy module.
/// </summary>
public sealed class TenantQuery : ITenantQuery
{
    public Task<Tenant?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Tenant read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Tenant>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Tenant paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Tenant uniqueness persistence has not been connected to the module DbContext.");
    }
}
