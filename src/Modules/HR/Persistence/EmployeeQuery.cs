using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Read-side persistence for Employee.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the HR module.
/// </summary>
public sealed class EmployeeQuery : IEmployeeQuery
{
    public Task<Employee?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Employee read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Employee>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Employee paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Employee uniqueness persistence has not been connected to the module DbContext.");
    }
}
