using System.Threading.Tasks;
using SmartSchool.Modules.Organization.Models;

namespace SmartSchool.Modules.Organization.Features.Campus;

/// <summary>
/// Defines command persistence operations for CampusEntity.
/// </summary>
public interface ICampusCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        CampusEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        CampusEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        CampusEntity entity,
        CancellationToken cancellationToken);
}
