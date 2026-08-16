using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

public interface IDocumentTemplateCommand
{
    Task AddAsync(
        DocumentTemplate entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        DocumentTemplate entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        DocumentTemplate entity,
        CancellationToken cancellationToken);
}
