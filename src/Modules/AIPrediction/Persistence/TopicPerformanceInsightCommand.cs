using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for TopicPerformanceInsightEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TopicPerformanceInsightCommand : ITopicPerformanceInsightCommand
{
    public Task AddAsync(
        TopicPerformanceInsightEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsightEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TopicPerformanceInsightEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsightEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TopicPerformanceInsightEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsightEntity delete persistence has not been connected to the module DbContext.");
    }
}
