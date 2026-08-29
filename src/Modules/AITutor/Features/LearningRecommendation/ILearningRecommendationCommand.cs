using System.Threading.Tasks;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.LearningRecommendation;

/// <summary>
/// Defines command persistence operations for LearningRecommendationEntity.
/// </summary>
public interface ILearningRecommendationCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		LearningRecommendationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		LearningRecommendationEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		LearningRecommendationEntity entity,
		CancellationToken cancellationToken);
}
