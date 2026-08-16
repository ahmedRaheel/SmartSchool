using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Persistence;

public interface IGeneratedDocumentQuery
{
    Task<GeneratedDocument?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    Task<PagedResult<GeneratedDocument>> GetPageAsync(
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
