using SmartSchool.Modules.Students.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Attendance;

/// <summary>
/// Executes database writes for <see cref="AttendanceEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AttendanceCommand(IStudentsDbContext dbContext) : IAttendanceCommand
{
	public async Task AddAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Attendances
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Attendances
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AttendanceEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Attendances
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
