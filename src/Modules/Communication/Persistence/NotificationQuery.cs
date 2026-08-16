using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Read-side persistence for Notification.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Communication module.
/// </summary>
public sealed class NotificationQuery : INotificationQuery
{
    public Task<Notification?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Notification read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<Notification>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Notification paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Notification uniqueness persistence has not been connected to the module DbContext.");
    }
}
