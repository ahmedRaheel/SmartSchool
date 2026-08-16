using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for Position.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PositionCommand : IPositionCommand
{
    public Task AddAsync(
        Position entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Position create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Position entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Position update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Position entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Position delete persistence has not been connected to the module DbContext.");
    }
}
