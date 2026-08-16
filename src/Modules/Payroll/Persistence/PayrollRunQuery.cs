using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Read-side persistence for PayrollRun.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Payroll module.
/// </summary>
public sealed class PayrollRunQuery : IPayrollRunQuery
{
    public Task<PayrollRun?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRun read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<PayrollRun>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRun paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PayrollRun uniqueness persistence has not been connected to the module DbContext.");
    }
}
