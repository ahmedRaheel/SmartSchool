using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.TimetableEntry;

/// <summary>
/// Executes database writes for <see cref="TimetableEntryEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TimetableEntryCommand(IApplicationDbContext dbContext) : ITimetableEntryCommand
{
	public async Task AddAsync(
		TimetableEntryEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<TimetableEntryEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TimetableEntryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TimetableEntryEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TimetableEntryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TimetableEntryEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
