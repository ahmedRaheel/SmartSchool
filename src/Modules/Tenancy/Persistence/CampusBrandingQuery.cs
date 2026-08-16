using SmartSchool.Modules.Tenancy.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Tenancy.Persistence;

/// <summary>
/// Read-side persistence for CampusBranding.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Tenancy module.
/// </summary>
public sealed class CampusBrandingQuery : ICampusBrandingQuery
{
    public Task<CampusBranding?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBranding read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<CampusBranding>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBranding paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusBranding uniqueness persistence has not been connected to the module DbContext.");
    }
}
