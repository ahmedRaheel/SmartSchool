using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for BookCopy.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class BookCopyCommand : IBookCopyCommand
{
    public Task AddAsync(
        BookCopy entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopy create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        BookCopy entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopy update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        BookCopy entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopy delete persistence has not been connected to the module DbContext.");
    }
}
