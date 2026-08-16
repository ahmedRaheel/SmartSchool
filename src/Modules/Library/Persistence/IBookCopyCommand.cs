using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

public interface IBookCopyCommand
{
    Task AddAsync(
        BookCopy entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        BookCopy entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        BookCopy entity,
        CancellationToken cancellationToken);
}
