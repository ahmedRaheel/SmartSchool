using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database writes for <see cref="CourseSelectionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CourseSelectionCommand(IApplicationDbContext dbContext) : ICourseSelectionCommand
{
	public async Task AddAsync(
		CourseSelectionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<CourseSelectionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		CourseSelectionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CourseSelectionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		CourseSelectionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CourseSelectionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
