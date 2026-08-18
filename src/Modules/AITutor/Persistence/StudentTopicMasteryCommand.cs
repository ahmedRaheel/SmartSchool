using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Executes database writes for <see cref="StudentTopicMasteryEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentTopicMasteryCommand(IApplicationDbContext dbContext) : IStudentTopicMasteryCommand
{
	public async Task AddAsync(
		StudentTopicMasteryEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentTopicMasteryEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentTopicMasteryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentTopicMasteryEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentTopicMasteryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentTopicMasteryEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
