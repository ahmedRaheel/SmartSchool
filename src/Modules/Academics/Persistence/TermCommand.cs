using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for TermEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TermCommand : ITermCommand
{
    public Task AddAsync(
        TermEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TermEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TermEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TermEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TermEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TermEntity delete persistence has not been connected to the module DbContext.");
    }
}
