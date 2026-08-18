using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Executes database writes for <see cref="PositionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PositionCommand(IApplicationDbContext dbContext) : IPositionCommand
{
	public async Task AddAsync(
		PositionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PositionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PositionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PositionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PositionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PositionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
