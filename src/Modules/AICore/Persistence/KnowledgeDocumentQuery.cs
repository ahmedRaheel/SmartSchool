using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Read-side persistence for KnowledgeDocumentEntity.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AICore module.
/// </summary>
public sealed class KnowledgeDocumentQuery : IKnowledgeDocumentQuery
{
    public Task<KnowledgeDocumentEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocumentEntity read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<KnowledgeDocumentEntity>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocumentEntity paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeDocumentEntity uniqueness persistence has not been connected to the module DbContext.");
    }
}
