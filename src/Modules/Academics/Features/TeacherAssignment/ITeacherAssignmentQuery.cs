using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Features.TeacherAssignment;

/// <summary>
/// Defines query persistence operations for TeacherAssignmentEntity.
/// </summary>
public interface ITeacherAssignmentQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<TeacherAssignmentEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<TeacherAssignmentEntity>> GetPageAsync(
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
