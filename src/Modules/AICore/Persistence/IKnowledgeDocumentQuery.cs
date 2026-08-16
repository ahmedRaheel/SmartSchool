using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IKnowledgeDocumentQuery
{
    Task<KnowledgeDocument?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<KnowledgeDocument>> GetPageAsync(
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
