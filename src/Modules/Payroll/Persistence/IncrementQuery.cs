using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Read-side persistence for Increment.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Payroll module.
/// </summary>
public sealed class IncrementQuery : IIncrementQuery
{
    public Task<Increment?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Increment read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Increment>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Increment paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Increment uniqueness persistence has not been connected to the module DbContext.");
    }
}
