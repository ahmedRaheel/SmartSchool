using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

public interface IBookCommand
{
    Task AddAsync(
        Book entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Book entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Book entity,
        CancellationToken cancellationToken);
}
