using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Executes database writes for <see cref="IncrementEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class IncrementCommand(IApplicationDbContext dbContext) : IIncrementCommand
{
	public async Task AddAsync(
		IncrementEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<IncrementEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		IncrementEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<IncrementEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		IncrementEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<IncrementEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
