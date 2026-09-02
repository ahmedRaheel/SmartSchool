using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.StudentPerformancePrediction;

/// <summary>
/// Defines query persistence operations for StudentPerformancePredictionEntity.
/// </summary>
public interface IStudentPerformancePredictionQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<StudentPerformancePredictionEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<StudentPerformancePredictionEntity>> GetPageAsync(
		Guid tenantId,
		int page,
		int pageSize,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	/// <summary>Gets records for one student within the authenticated tenant.</summary>
	Task<IReadOnlyCollection<StudentPerformancePredictionEntity>> GetByStudentIdAsync(
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
