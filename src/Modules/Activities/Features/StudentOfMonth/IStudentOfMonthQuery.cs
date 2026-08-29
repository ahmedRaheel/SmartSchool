using System.Threading.Tasks;
using SmartSchool.Modules.Activities.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Activities.Features.StudentOfMonth;

/// <summary>
/// Defines query persistence operations for StudentOfMonthEntity.
/// </summary>
public interface IStudentOfMonthQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<StudentOfMonthEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<StudentOfMonthEntity>> GetPageAsync(
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
