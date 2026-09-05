using System.Threading.Tasks;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Features.Vehicle;

/// <summary>
/// Defines command persistence operations for VehicleEntity.
/// </summary>
public interface IVehicleCommand
{
    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task AddAsync(
        VehicleEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task UpdateAsync(
        VehicleEntity entity,
        CancellationToken cancellationToken);

    /// <summary>
    /// Executes the persistence operation.
    /// </summary>
    Task DeleteAsync(
        VehicleEntity entity,
        CancellationToken cancellationToken);
}
