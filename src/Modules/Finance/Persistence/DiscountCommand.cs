using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for DiscountEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class DiscountCommand : IDiscountCommand
{
    public Task AddAsync(
        DiscountEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DiscountEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        DiscountEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DiscountEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        DiscountEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "DiscountEntity delete persistence has not been connected to the module DbContext.");
    }
}
