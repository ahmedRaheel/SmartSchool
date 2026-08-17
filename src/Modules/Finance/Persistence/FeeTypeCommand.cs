using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for FeeTypeEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class FeeTypeCommand : IFeeTypeCommand
{
    public Task AddAsync(
        FeeTypeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeTypeEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        FeeTypeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeTypeEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        FeeTypeEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeTypeEntity delete persistence has not been connected to the module DbContext.");
    }
}
