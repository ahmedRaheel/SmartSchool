using SmartSchool.Modules.AIPrediction.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database writes for <see cref="PredictionEvaluationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PredictionEvaluationCommand(IAIPredictionDbContext dbContext) : IPredictionEvaluationCommand
{
	public async Task AddAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.PredictionEvaluations
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.PredictionEvaluations
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.PredictionEvaluations
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
