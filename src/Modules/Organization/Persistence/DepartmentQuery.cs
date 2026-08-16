using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Persistence;

/// <summary>
/// Read-side persistence for Department.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Organization module.
/// </summary>
public sealed class DepartmentQuery : IDepartmentQuery
{
    public Task<Department?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Department read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Department>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Department paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Department uniqueness persistence has not been connected to the module DbContext.");
    }
}
