using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Features.CourseOffering;

/// <summary>
/// Defines query persistence operations for CourseOfferingEntity.
/// </summary>
public interface ICourseOfferingQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<CourseOfferingEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<CourseOfferingEntity>> GetPageAsync(
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
