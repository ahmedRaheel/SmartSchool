using System.Threading.Tasks;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Features.AssignmentSubmission;

/// <summary>
/// Defines query persistence operations for AssignmentSubmissionEntity.
/// </summary>
public interface IAssignmentSubmissionQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<AssignmentSubmissionEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<AssignmentSubmissionEntity>> GetPageAsync(
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
