using System.Threading.Tasks;
using SmartSchool.Modules.Workflow.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Workflow.Features.WorkflowInstance;

/// <summary>
/// Defines query persistence operations for WorkflowInstanceEntity.
/// </summary>
public interface IWorkflowInstanceQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<WorkflowInstanceEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<WorkflowInstanceEntity>> GetPageAsync(
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
