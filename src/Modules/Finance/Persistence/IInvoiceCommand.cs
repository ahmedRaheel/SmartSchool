using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IInvoiceCommand
{
    Task AddAsync(
        Invoice entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Invoice entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Invoice entity,
        CancellationToken cancellationToken);
}
