using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.ClassPerformanceInsight;

/// <summary>
/// Executes database writes for <see cref="ClassPerformanceInsightEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ClassPerformanceInsightCommand(IApplicationDbContext dbContext) : IClassPerformanceInsightCommand
{
	public async Task AddAsync(
		ClassPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ClassPerformanceInsightEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ClassPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ClassPerformanceInsightEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ClassPerformanceInsightEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ClassPerformanceInsightEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
