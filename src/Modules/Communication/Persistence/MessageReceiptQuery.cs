using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Read-side persistence for MessageReceipt.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Communication module.
/// </summary>
public sealed class MessageReceiptQuery : IMessageReceiptQuery
{
    public Task<MessageReceipt?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceipt read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<MessageReceipt>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceipt paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "MessageReceipt uniqueness persistence has not been connected to the module DbContext.");
    }
}
