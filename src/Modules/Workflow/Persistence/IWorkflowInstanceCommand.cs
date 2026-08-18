using System.Threading.Tasks;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Defines command persistence operations for WorkflowInstanceEntity.
/// </summary>
public interface IWorkflowInstanceCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		WorkflowInstanceEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		WorkflowInstanceEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		WorkflowInstanceEntity entity,
		CancellationToken cancellationToken);
}
