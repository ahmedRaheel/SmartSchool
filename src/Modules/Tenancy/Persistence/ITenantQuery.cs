using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

public interface ITenantQuery
{
    Task<Tenant?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Tenant>> GetPageAsync(
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
