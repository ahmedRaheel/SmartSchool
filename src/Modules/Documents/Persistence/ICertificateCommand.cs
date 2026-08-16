using SmartSchool.Modules.Documents.Models;

namespace SmartSchool.Modules.Documents.Persistence;

public interface ICertificateCommand
{
    Task AddAsync(
        Certificate entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Certificate entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Certificate entity,
        CancellationToken cancellationToken);
}
