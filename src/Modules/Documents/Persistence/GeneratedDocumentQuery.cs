using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Read-side persistence for GeneratedDocument.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Documents module.
/// </summary>
public sealed class GeneratedDocumentQuery : IGeneratedDocumentQuery
{
    public Task<GeneratedDocument?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocument read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<GeneratedDocument>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocument paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "GeneratedDocument uniqueness persistence has not been connected to the module DbContext.");
    }
}
