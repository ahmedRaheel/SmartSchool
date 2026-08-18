using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Executes database writes for <see cref="GuardianEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GuardianCommand(IApplicationDbContext dbContext) : IGuardianCommand
{
	public async Task AddAsync(
		GuardianEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<GuardianEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		GuardianEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GuardianEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		GuardianEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GuardianEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
