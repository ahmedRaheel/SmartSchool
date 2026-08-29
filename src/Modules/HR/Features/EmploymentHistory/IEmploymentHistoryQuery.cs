using System.Threading.Tasks;
using SmartSchool.Modules.HR.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.HR.Features.EmploymentHistory;

/// <summary>
/// Defines query persistence operations for EmploymentHistoryEntity.
/// </summary>
public interface IEmploymentHistoryQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<EmploymentHistoryEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<EmploymentHistoryEntity>> GetPageAsync(
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
