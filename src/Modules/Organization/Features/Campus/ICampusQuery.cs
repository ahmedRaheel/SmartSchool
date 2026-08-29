using System.Threading.Tasks;
using SmartSchool.Modules.Organization.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Organization.Features.Campus;

/// <summary>
/// Defines query persistence operations for CampusEntity.
/// </summary>
public interface ICampusQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<CampusEntity?> GetByIdAsync(
		Guid? tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<CampusEntity>> GetPageAsync(
		Guid? tenantId,
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
