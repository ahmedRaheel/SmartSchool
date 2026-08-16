using SmartSchool.Modules.Finance.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Read-side persistence for Payment.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Finance module.
/// </summary>
public sealed class PaymentQuery : IPaymentQuery
{
    public Task<Payment?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payment read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Payment>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payment paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payment uniqueness persistence has not been connected to the module DbContext.");
    }
}
