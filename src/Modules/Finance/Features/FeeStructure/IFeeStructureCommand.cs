using System.Threading.Tasks;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.FeeStructure;

/// <summary>
/// Defines command persistence operations for FeeStructureEntity.
/// </summary>
public interface IFeeStructureCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        FeeStructureEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        FeeStructureEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        FeeStructureEntity entity,
        CancellationToken cancellationToken);
}
