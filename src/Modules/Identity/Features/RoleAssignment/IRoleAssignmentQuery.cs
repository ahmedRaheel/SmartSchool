using System.Threading.Tasks;
using SmartSchool.Modules.Identity.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Identity.Features.RoleAssignment;

/// <summary>
/// Defines query persistence operations for RoleAssignmentEntity.
/// </summary>
public interface IRoleAssignmentQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<RoleAssignmentEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<RoleAssignmentEntity>> GetPageAsync(
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
