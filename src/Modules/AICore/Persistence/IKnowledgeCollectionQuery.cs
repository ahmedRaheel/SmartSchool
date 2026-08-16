using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

public interface IKnowledgeCollectionQuery
{
    Task<KnowledgeCollection?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<KnowledgeCollection>> GetPageAsync(
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
