using System.Threading.Tasks;
using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Features.BookCopy;

/// <summary>
/// Defines command persistence operations for BookCopyEntity.
/// </summary>
public interface IBookCopyCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        BookCopyEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        BookCopyEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        BookCopyEntity entity,
        CancellationToken cancellationToken);
}
