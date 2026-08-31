using SmartSchool.Modules.AIPrediction.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.TopicPerformanceInsight;

/// <summary>
/// Executes database writes for <see cref="TopicPerformanceInsightEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TopicPerformanceInsightCommand(IAIPredictionDbContext dbContext) : ITopicPerformanceInsightCommand
{
	public async Task AddAsync(
		TopicPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.TopicPerformanceInsights
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TopicPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TopicPerformanceInsights
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TopicPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TopicPerformanceInsights
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
