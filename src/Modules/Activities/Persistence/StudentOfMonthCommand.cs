using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Persistence;

/// <summary>
/// Executes database writes for <see cref="StudentOfMonthEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentOfMonthCommand(IApplicationDbContext dbContext) : IStudentOfMonthCommand
{
	public async Task AddAsync(
		StudentOfMonthEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentOfMonthEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentOfMonthEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentOfMonthEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentOfMonthEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentOfMonthEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
