using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Write-side persistence for Invoice.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class InvoiceCommand : IInvoiceCommand
{
    public Task AddAsync(
        Invoice entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Invoice create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        Invoice entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Invoice update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        Invoice entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "Invoice delete persistence has not been connected to the module DbContext.");
    }
}
