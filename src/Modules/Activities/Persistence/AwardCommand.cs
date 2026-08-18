using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Executes database writes for <see cref="AwardEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AwardCommand(IApplicationDbContext dbContext) : IAwardCommand
{
	public async Task AddAsync(
		AwardEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AwardEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AwardEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AwardEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AwardEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AwardEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
