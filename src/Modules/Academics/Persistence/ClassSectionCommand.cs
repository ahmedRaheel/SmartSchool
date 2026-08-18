using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database writes for <see cref="ClassSectionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ClassSectionCommand(IApplicationDbContext dbContext) : IClassSectionCommand
{
	public async Task AddAsync(
		ClassSectionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ClassSectionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ClassSectionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ClassSectionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ClassSectionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ClassSectionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
