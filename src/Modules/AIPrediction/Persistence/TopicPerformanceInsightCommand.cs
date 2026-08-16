using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Write-side persistence for TopicPerformanceInsight.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class TopicPerformanceInsightCommand : ITopicPerformanceInsightCommand
{
    public Task AddAsync(
        TopicPerformanceInsight entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsight create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        TopicPerformanceInsight entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsight update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        TopicPerformanceInsight entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "TopicPerformanceInsight delete persistence has not been connected to the module DbContext.");
    }
}
