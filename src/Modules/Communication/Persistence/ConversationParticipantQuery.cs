using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Read-side persistence for ConversationParticipant.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Communication module.
/// </summary>
public sealed class ConversationParticipantQuery : IConversationParticipantQuery
{
    public Task<ConversationParticipant?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipant read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<ConversationParticipant>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipant paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "ConversationParticipant uniqueness persistence has not been connected to the module DbContext.");
    }
}
