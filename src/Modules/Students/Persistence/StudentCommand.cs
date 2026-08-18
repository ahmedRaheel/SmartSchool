using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Executes database writes for <see cref="StudentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentCommand(IApplicationDbContext dbContext) : IStudentCommand
{
	public async Task AddAsync(
		StudentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
