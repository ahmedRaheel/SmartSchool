using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for FeeStructureEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class FeeStructureCommand : IFeeStructureCommand
{
    public Task AddAsync(
        FeeStructureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeStructureEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        FeeStructureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeStructureEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        FeeStructureEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeStructureEntity delete persistence has not been connected to the module DbContext.");
    }
}
