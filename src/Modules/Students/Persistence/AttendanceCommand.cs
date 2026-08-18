using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Executes database writes for <see cref="AttendanceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AttendanceCommand(IApplicationDbContext dbContext) : IAttendanceCommand
{
	public async Task AddAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AttendanceEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AttendanceEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AttendanceEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
