using SmartSchool.Modules.AITutor.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Read-side persistence for TutorConversation.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AITutor module.
/// </summary>
public sealed class TutorConversationQuery : ITutorConversationQuery
{
    public Task<TutorConversation?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversation read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<TutorConversation>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversation paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TutorConversation uniqueness persistence has not been connected to the module DbContext.");
    }
}
