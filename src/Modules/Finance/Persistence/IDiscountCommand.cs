using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IDiscountCommand
{
    Task AddAsync(
        Discount entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        Discount entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        Discount entity,
        CancellationToken cancellationToken);
}
