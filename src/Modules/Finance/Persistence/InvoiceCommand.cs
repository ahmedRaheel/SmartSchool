using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for InvoiceEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InvoiceCommand : IInvoiceCommand
{
    public Task AddAsync(
        InvoiceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InvoiceEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        InvoiceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InvoiceEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        InvoiceEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "InvoiceEntity delete persistence has not been connected to the module DbContext.");
    }
}
