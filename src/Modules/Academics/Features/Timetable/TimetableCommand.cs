using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.Timetable;

/// <summary>
/// Executes database writes for <see cref="TimetableEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TimetableCommand(IApplicationDbContext dbContext) : ITimetableCommand
{
	public async Task AddAsync(
		TimetableEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<TimetableEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TimetableEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TimetableEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TimetableEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TimetableEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
