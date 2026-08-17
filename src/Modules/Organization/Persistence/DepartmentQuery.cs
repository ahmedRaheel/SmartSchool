using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Read-side persistence for DepartmentEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Organization module.
/// </summary>
public sealed class DepartmentQuery : IDepartmentQuery
{
    public Task<DepartmentEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DepartmentEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<DepartmentEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DepartmentEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DepartmentEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
