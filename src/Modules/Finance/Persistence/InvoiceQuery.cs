using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Read-side persistence for Invoice.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Finance module.
/// </summary>
public sealed class InvoiceQuery : IInvoiceQuery
{
    public Task<Invoice?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Invoice read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Invoice>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Invoice paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Invoice uniqueness persistence has not been connected to the module DbContext.");
    }
}
