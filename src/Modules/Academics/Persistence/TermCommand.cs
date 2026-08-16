using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Write-side persistence for Term.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TermCommand : ITermCommand
{
    public Task AddAsync(
        Term entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Term create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Term entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Term update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Term entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Term delete persistence has not been connected to the module DbContext.");
    }
}
