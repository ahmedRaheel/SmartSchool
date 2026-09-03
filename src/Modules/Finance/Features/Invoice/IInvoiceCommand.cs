using System.Threading.Tasks;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.Invoice;

/// <summary>
/// Defines command persistence operations for InvoiceEntity.
/// </summary>
public interface IInvoiceCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        InvoiceEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        InvoiceEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        InvoiceEntity entity,
        CancellationToken cancellationToken);
}
