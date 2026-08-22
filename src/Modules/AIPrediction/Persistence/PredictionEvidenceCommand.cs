using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database writes for <see cref="PredictionEvidenceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PredictionEvidenceCommand(IApplicationDbContext dbContext) : IPredictionEvidenceCommand
{
	public async Task AddAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PredictionEvidenceEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PredictionEvidenceEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PredictionEvidenceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PredictionEvidenceEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
