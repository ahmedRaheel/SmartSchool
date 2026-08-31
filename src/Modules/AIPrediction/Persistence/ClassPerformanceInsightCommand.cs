using SmartSchool.Modules.AIPrediction.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database writes for <see cref="ClassPerformanceInsightEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ClassPerformanceInsightCommand(IAIPredictionDbContext dbContext) : IClassPerformanceInsightCommand
{
	public async Task AddAsync(
		ClassPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.ClassPerformanceInsights
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ClassPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ClassPerformanceInsights
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ClassPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ClassPerformanceInsights
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
