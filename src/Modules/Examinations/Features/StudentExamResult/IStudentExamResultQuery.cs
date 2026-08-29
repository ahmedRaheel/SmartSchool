using System.Threading.Tasks;
using SmartSchool.Modules.Examinations.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

/// <summary>
/// Defines query persistence operations for StudentExamResultEntity.
/// </summary>
public interface IStudentExamResultQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<StudentExamResultEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<StudentExamResultEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	/// <summary>Gets records for one student within the authenticated tenant.</summary>
	Task<IReadOnlyCollection<StudentExamResultEntity>> GetByStudentIdAsync(
		Guid tenantId,
		Guid studentId,
		int limit,
		CancellationToken cancellationToken);

	Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken);
}
