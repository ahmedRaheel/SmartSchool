using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for Discount.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class DiscountCommand : IDiscountCommand
{
    public Task AddAsync(
        Discount entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Discount create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Discount entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Discount update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Discount entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Discount delete persistence has not been connected to the module DbContext.");
    }
}
