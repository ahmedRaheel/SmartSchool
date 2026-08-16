using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

public interface IConversationQuery
{
    Task<Conversation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<Conversation>> GetPageAsync(
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
