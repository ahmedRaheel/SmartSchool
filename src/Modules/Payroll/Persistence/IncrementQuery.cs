using SmartSchool.Modules.Payroll.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Read-side persistence for IncrementEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Payroll module.
/// </summary>
public sealed class IncrementQuery : IIncrementQuery
{
    public Task<IncrementEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "IncrementEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<IncrementEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "IncrementEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "IncrementEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
