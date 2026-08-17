using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for BookCopyEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class BookCopyCommand : IBookCopyCommand
{
    public Task AddAsync(
        BookCopyEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopyEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        BookCopyEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopyEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        BookCopyEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookCopyEntity delete persistence has not been connected to the module DbContext.");
    }
}
