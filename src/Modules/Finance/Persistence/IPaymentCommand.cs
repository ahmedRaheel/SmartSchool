using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IPaymentCommand
{
    Task AddAsync(
        Payment entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Payment entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Payment entity,
        CancellationToken cancellationToken);
}
