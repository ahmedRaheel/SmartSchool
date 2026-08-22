using System.Threading.Tasks;
using SmartSchool.Modules.Identity.Models;

namespace SmartSchool.Modules.Identity.Persistence;

/// <summary>
/// Defines command persistence operations for RoleAssignmentEntity.
/// </summary>
public interface IRoleAssignmentCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		RoleAssignmentEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		RoleAssignmentEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		RoleAssignmentEntity entity,
		CancellationToken cancellationToken);
}
