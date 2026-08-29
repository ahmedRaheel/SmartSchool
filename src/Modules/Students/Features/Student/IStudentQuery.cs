using System.Threading.Tasks;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Student;

/// <summary>
/// Defines query persistence operations for StudentEntity.
/// </summary>
public interface IStudentQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<StudentEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<StudentEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<bool> ExistsByStudentNumberAsync(
		Guid tenantId,
		string studentNumber,
		Guid? excludingId,
		CancellationToken cancellationToken);
}
