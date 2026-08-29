using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.Interview;

/// <summary>
/// Defines query persistence operations for InterviewEntity.
/// </summary>
public interface IInterviewQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<InterviewEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<InterviewEntity>> GetPageAsync(
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
