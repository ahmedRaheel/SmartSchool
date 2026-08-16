using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

public interface IFeeStructureCommand
{
    Task AddAsync(
        FeeStructure entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        FeeStructure entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        FeeStructure entity,
        CancellationToken cancellationToken);
}
