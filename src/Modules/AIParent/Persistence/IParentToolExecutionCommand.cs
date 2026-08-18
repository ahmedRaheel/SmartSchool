using System.Threading.Tasks;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Defines command persistence operations for ParentToolExecutionEntity.
/// </summary>
public interface IParentToolExecutionCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		ParentToolExecutionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		ParentToolExecutionEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		ParentToolExecutionEntity entity,
		CancellationToken cancellationToken);
}
