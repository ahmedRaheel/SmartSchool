using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Executes database writes for <see cref="ActivityEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ActivityCommand(IApplicationDbContext dbContext) : IActivityCommand
{
	public async Task AddAsync(
		ActivityEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ActivityEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ActivityEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ActivityEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ActivityEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ActivityEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
