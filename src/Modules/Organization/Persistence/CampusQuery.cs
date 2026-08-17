using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Read-side persistence for CampusEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Organization module.
/// </summary>
public sealed class CampusQuery : ICampusQuery
{
    public Task<CampusEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<CampusEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "CampusEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
