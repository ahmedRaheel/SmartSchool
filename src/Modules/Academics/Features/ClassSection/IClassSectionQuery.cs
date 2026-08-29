using System.Threading.Tasks;
using SmartSchool.Modules.Academics.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.Academics.Features.ClassSection;

/// <summary>
/// Defines query persistence operations for ClassSectionEntity.
/// </summary>
public interface IClassSectionQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<ClassSectionEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<ClassSectionEntity>> GetPageAsync(
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
