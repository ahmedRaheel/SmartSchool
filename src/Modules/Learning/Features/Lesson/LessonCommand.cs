using SmartSchool.Modules.Learning.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Features.Lesson;

/// <summary>
/// Executes database writes for <see cref="LessonEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LessonCommand(ILearningDbContext dbContext) : ILessonCommand
{
	public async Task AddAsync(
		LessonEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Lessons
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		LessonEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Lessons
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		LessonEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Lessons
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
