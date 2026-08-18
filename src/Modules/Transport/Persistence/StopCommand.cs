using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Executes database writes for <see cref="StopEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StopCommand(IApplicationDbContext dbContext) : IStopCommand
{
	public async Task AddAsync(
		StopEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StopEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StopEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StopEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StopEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StopEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
