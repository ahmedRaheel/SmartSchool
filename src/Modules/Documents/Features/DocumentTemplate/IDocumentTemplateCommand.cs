using System.Threading.Tasks;
using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Features.DocumentTemplate;

/// <summary>
/// Defines command persistence operations for DocumentTemplateEntity.
/// </summary>
public interface IDocumentTemplateCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        DocumentTemplateEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        DocumentTemplateEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        DocumentTemplateEntity entity,
        CancellationToken cancellationToken);
}
