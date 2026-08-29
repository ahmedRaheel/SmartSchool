using System.Threading.Tasks;
using SmartSchool.Modules.Students.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Students.Features.Enrollment;

/// <summary>
/// Defines query persistence operations for EnrollmentEntity.
/// </summary>
public interface IEnrollmentQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<EnrollmentEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<EnrollmentEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	Task<bool> ExistsForAcademicYearAsync(
		Guid tenantId,
		Guid studentId,
		Guid academicYearId,
		CancellationToken cancellationToken);
}
