using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

public interface IGeneratedDocumentCommand
{
    Task AddAsync(
        GeneratedDocument entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        GeneratedDocument entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        GeneratedDocument entity,
        CancellationToken cancellationToken);
}
