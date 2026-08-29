using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;
using SmartSchool.SharedKernel;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

/// <summary>
/// Defines query persistence operations for TeachingRecommendationEntity.
/// </summary>
public interface ITeachingRecommendationQuery
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<TeachingRecommendationEntity?> GetByIdAsync(
		Guid tenantId,
		Guid id,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task<PagedResult<TeachingRecommendationEntity>> GetPageAsync(
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
