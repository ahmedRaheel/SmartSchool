using System.Threading.Tasks;
using SmartSchool.Modules.Learning.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Learning.Features.Assignment;

/// <summary>
/// Defines query persistence operations for AssignmentEntity.
/// </summary>
public interface IAssignmentQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<AssignmentEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<AssignmentEntity>> GetPageAsync(
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
