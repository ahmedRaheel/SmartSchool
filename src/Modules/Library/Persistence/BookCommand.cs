using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for Book.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class BookCommand : IBookCommand
{
    public Task AddAsync(
        Book entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Book create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Book entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Book update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Book entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Book delete persistence has not been connected to the module DbContext.");
    }
}
