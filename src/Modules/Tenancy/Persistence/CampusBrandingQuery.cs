using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Read-side persistence for CampusBrandingEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Tenancy module.
/// </summary>
public sealed class CampusBrandingQuery : ICampusBrandingQuery
{
    public Task<CampusBrandingEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBrandingEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<CampusBrandingEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBrandingEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBrandingEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
