using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Defines query persistence operations for AcademicYearEntity.
/// </summary>
public interface IAcademicYearQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<AcademicYearEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<AcademicYearEntity>> GetPageAsync(
		Guid tenantId,
		Guid campusId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<bool> CampusExistsAsync(
		Guid tenantId,
		Guid campusId,
		CancellationToken cancellationToken);

	Task<bool> ExistsByCodeAsync(
		Guid tenantId,
		string code,
		Guid? excludingId,
		CancellationToken cancellationToken);
}
