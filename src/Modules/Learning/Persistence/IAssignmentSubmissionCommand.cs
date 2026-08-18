using System.Threading.Tasks;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Defines command persistence operations for AssignmentSubmissionEntity.
/// </summary>
public interface IAssignmentSubmissionCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		AssignmentSubmissionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		AssignmentSubmissionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		AssignmentSubmissionEntity entity,
		CancellationToken cancellationToken);
}
