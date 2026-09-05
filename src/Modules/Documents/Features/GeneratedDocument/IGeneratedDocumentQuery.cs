using System.Threading.Tasks;
using SmartSchool.Modules.Documents.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Documents.Features.GeneratedDocument;

/// <summary>
/// Defines query persistence operations for GeneratedDocumentEntity.
/// </summary>
public interface IGeneratedDocumentQuery
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<GeneratedDocumentEntity?> GetByIdAsync(
        Guid tenantId,
        Guid id,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task<PagedResult<GeneratedDocumentEntity>> GetPageAsync(
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
