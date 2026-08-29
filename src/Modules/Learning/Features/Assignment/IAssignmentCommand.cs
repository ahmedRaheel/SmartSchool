using System.Threading.Tasks;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Features.Assignment;

/// <summary>
/// Defines command persistence operations for AssignmentEntity.
/// </summary>
public interface IAssignmentCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		AssignmentEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		AssignmentEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		AssignmentEntity entity,
		CancellationToken cancellationToken);
}
