using System.Threading.Tasks;
using SmartSchool.Modules.Communication.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Communication.Features.ConversationParticipant;

/// <summary>
/// Defines query persistence operations for ConversationParticipantEntity.
/// </summary>
public interface IConversationParticipantQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<ConversationParticipantEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<ConversationParticipantEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken);
}
