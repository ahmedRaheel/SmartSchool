using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for FeeType.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class FeeTypeCommand : IFeeTypeCommand
{
    public Task AddAsync(
        FeeType entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeType create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        FeeType entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeType update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        FeeType entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeType delete persistence has not been connected to the module DbContext.");
    }
}
