using SmartSchool.Modules.AICore.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Read-side persistence for KnowledgeChunk.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the AICore module.
/// </summary>
public sealed class KnowledgeChunkQuery : IKnowledgeChunkQuery
{
    public Task<KnowledgeChunk?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunk read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<KnowledgeChunk>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunk paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeChunk uniqueness persistence has not been connected to the module DbContext.");
    }
}
