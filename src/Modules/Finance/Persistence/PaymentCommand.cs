using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for PaymentEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class PaymentCommand : IPaymentCommand
{
    public Task AddAsync(
        PaymentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PaymentEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        PaymentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PaymentEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        PaymentEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "PaymentEntity delete persistence has not been connected to the module DbContext.");
    }
}
