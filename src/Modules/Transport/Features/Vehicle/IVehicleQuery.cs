using System.Threading.Tasks;
using SmartSchool.Modules.Transport.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Transport.Features.Vehicle;

/// <summary>
/// Defines query persistence operations for VehicleEntity.
/// </summary>
public interface IVehicleQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<VehicleEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<VehicleEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken);
}
