using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for Increment.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class IncrementCommand : IIncrementCommand
{
    public Task AddAsync(
        Increment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Increment create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Increment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Increment update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Increment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Increment delete persistence has not been connected to the module DbContext.");
    }
}
