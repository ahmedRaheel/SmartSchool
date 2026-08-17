using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Write-side persistence for PositionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PositionCommand : IPositionCommand
{
    public Task AddAsync(
        PositionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PositionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PositionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PositionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PositionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PositionEntity delete persistence has not been connected to the module DbContext.");
    }
}
