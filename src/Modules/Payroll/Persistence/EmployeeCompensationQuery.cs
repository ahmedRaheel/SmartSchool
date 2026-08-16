using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Read-side persistence for EmployeeCompensation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Payroll module.
/// </summary>
public sealed class EmployeeCompensationQuery : IEmployeeCompensationQuery
{
    public Task<EmployeeCompensation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<EmployeeCompensation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "EmployeeCompensation uniqueness persistence has not been connected to the module DbContext.");
    }
}
