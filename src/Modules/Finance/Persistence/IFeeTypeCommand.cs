using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IFeeTypeCommand
{
    Task AddAsync(
        FeeType entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        FeeType entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        FeeType entity,
        CancellationToken cancellationToken);
}
