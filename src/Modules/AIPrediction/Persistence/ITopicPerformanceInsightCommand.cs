using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface ITopicPerformanceInsightCommand
{
    Task AddAsync(
        TopicPerformanceInsight entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        TopicPerformanceInsight entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        TopicPerformanceInsight entity,
        CancellationToken cancellationToken);
}
