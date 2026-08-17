using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Write-side persistence for IncrementEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class IncrementCommand : IIncrementCommand
{
    public Task AddAsync(
        IncrementEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "IncrementEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        IncrementEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "IncrementEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        IncrementEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "IncrementEntity delete persistence has not been connected to the module DbContext.");
    }
}
