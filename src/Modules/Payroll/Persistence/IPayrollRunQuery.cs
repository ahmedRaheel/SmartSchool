using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

public interface IPayrollRunQuery
{
    Task<PayrollRun?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<PayrollRun>> GetPageAsync(
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
