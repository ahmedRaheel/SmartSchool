using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.GradeScale;

/// <summary>
/// Executes database writes for <see cref="GradeScaleEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class GradeScaleCommand(IApplicationDbContext dbContext) : IGradeScaleCommand
{
	public async Task AddAsync(
		GradeScaleEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<GradeScaleEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		GradeScaleEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GradeScaleEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		GradeScaleEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<GradeScaleEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
