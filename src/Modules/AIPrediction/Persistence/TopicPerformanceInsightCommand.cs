using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// EF-backed write persistence for TopicPerformanceInsightEntity.
/// </summary>
public sealed class TopicPerformanceInsightCommand(IEfMockStore store) : ITopicPerformanceInsightCommand
{
	public Task AddAsync(TopicPerformanceInsightEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(TopicPerformanceInsightEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(TopicPerformanceInsightEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
