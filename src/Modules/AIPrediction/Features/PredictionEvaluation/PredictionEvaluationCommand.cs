using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Features.PredictionEvaluation;

/// <summary>
/// Executes database writes for <see cref="PredictionEvaluationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PredictionEvaluationCommand(IApplicationDbContext dbContext) : IPredictionEvaluationCommand
{
	public async Task AddAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PredictionEvaluationEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PredictionEvaluationEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PredictionEvaluationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PredictionEvaluationEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
