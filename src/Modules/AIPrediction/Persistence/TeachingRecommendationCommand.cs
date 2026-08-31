using SmartSchool.Modules.AIPrediction.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database writes for <see cref="TeachingRecommendationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TeachingRecommendationCommand(IAIPredictionDbContext dbContext) : ITeachingRecommendationCommand
{
	public async Task AddAsync(
		TeachingRecommendationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.TeachingRecommendations
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TeachingRecommendationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TeachingRecommendations
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TeachingRecommendationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TeachingRecommendations
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
