using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database writes for <see cref="TeacherAssignmentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TeacherAssignmentCommand(IApplicationDbContext dbContext) : ITeacherAssignmentCommand
{
	public async Task AddAsync(
		TeacherAssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<TeacherAssignmentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TeacherAssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TeacherAssignmentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TeacherAssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TeacherAssignmentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
