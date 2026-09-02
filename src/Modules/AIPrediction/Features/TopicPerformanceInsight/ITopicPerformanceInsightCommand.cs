using System.Threading.Tasks;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;

/// <summary>
/// Defines command persistence operations for TopicPerformanceInsightEntity.
/// </summary>
public interface ITopicPerformanceInsightCommand
{
	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task AddAsync(
		TopicPerformanceInsightEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task UpdateAsync(
		TopicPerformanceInsightEntity entity,
		CancellationToken cancellationToken);

	/// <summary>
	/// Executes the persistence operation.
	/// </summary>
	Task DeleteAsync(
		TopicPerformanceInsightEntity entity,
		CancellationToken cancellationToken);
}
