using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Features.CourseOffering;

/// <summary>
/// Executes database writes for <see cref="CourseOfferingEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CourseOfferingCommand(IApplicationDbContext dbContext) : ICourseOfferingCommand
{
	public async Task AddAsync(
		CourseOfferingEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<CourseOfferingEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		CourseOfferingEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CourseOfferingEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		CourseOfferingEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CourseOfferingEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
