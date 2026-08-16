using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

public interface IClassPerformanceInsightCommand
{
    Task AddAsync(
        ClassPerformanceInsight entity,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        ClassPerformanceInsight entity,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        ClassPerformanceInsight entity,
        CancellationToken cancellationToken);
}
