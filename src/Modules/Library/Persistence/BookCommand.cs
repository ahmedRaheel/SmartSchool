using SmartSchool.Modules.Library.Models;

namespace SmartSchool.Modules.Library.Persistence;

/// <summary>
/// Write-side persistence for BookEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class BookCommand : IBookCommand
{
    public Task AddAsync(
        BookEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        BookEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        BookEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "BookEntity delete persistence has not been connected to the module DbContext.");
    }
}
