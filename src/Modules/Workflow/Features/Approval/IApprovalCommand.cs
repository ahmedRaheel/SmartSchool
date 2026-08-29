using System.Threading.Tasks;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Features.Approval;

/// <summary>
/// Defines command persistence operations for ApprovalEntity.
/// </summary>
public interface IApprovalCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		ApprovalEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		ApprovalEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		ApprovalEntity entity,
		CancellationToken cancellationToken);
}
