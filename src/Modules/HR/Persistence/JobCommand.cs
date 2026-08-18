using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Executes database writes for <see cref="JobEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class JobCommand(IApplicationDbContext dbContext) : IJobCommand
{
	public async Task AddAsync(
		JobEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<JobEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		JobEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<JobEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		JobEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<JobEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
