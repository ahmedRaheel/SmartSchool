using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.TeachingRecommendation;

/// <summary>
/// Defines command persistence operations for TeachingRecommendationEntity.
/// </summary>
public interface ITeachingRecommendationCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		TeachingRecommendationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		TeachingRecommendationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		TeachingRecommendationEntity entity,
		CancellationToken cancellationToken);
}
