using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for FeeStructure.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class FeeStructureCommand : IFeeStructureCommand
{
    public Task AddAsync(
        FeeStructure entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeStructure create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        FeeStructure entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeStructure update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        FeeStructure entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "FeeStructure delete persistence has not been connected to the module DbContext.");
    }
}
