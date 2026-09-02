using System.Threading.Tasks;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Features.WorkflowStep;

/// <summary>
/// Defines command persistence operations for WorkflowStepEntity.
/// </summary>
public interface IWorkflowStepCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		WorkflowStepEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		WorkflowStepEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		WorkflowStepEntity entity,
		CancellationToken cancellationToken);
}
