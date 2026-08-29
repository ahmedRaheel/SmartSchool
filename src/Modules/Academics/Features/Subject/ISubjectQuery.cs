using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Features.Subject;

/// <summary>
/// Defines query persistence operations for SubjectEntity.
/// </summary>
public interface ISubjectQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<SubjectEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<SubjectEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	Task<string?> GetBranchCodeAsync(
		Guid tenantId,
		Guid branchId,
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
