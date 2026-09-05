using System.Threading.Tasks;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.Payment;

/// <summary>
/// Defines command persistence operations for PaymentEntity.
/// </summary>
public interface IPaymentCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        PaymentEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        PaymentEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        PaymentEntity entity,
        CancellationToken cancellationToken);
}
