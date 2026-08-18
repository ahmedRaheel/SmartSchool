using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database writes for <see cref="PredictionModelEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PredictionModelCommand(IApplicationDbContext dbContext) : IPredictionModelCommand
{
	public async Task AddAsync(
		PredictionModelEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PredictionModelEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PredictionModelEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PredictionModelEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PredictionModelEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PredictionModelEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
