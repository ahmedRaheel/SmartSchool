using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for Payment.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PaymentCommand : IPaymentCommand
{
    public Task AddAsync(
        Payment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payment create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Payment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payment update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Payment entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Payment delete persistence has not been connected to the module DbContext.");
    }
}
