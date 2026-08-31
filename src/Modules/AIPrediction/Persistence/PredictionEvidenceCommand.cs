using SmartSchool.Modules.AIPrediction.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database writes for <see cref="PredictionEvidenceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PredictionEvidenceCommand(IAIPredictionDbContext dbContext) : IPredictionEvidenceCommand
{
	public async Task AddAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.PredictionEvidences
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.PredictionEvidences
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.PredictionEvidences
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
