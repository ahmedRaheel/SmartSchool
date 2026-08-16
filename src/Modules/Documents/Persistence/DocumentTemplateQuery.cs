using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

/// <summary>
/// Read-side persistence for DocumentTemplate.
/// Replace the scaffolded methods with optimized EF Core/Dapper queries
/// owned by the Documents module.
/// </summary>
public sealed class DocumentTemplateQuery : IDocumentTemplateQuery
{
    public Task<DocumentTemplate?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplate read persistence has not been connected to the module DbContext.");
    }

    public Task<PagedResult<DocumentTemplate>> GetPageAsync(
        Guid tenantId,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplate paging persistence has not been connected to the module DbContext.");
    }

    public Task<bool> ExistsByCodeAsync(
        Guid tenantId,
        string code,
        Guid? excludingId,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DocumentTemplate uniqueness persistence has not been connected to the module DbContext.");
    }
}
