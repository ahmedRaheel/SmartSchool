using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database writes for <see cref="AcademicYearEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AcademicYearCommand(IApplicationDbContext dbContext) : IAcademicYearCommand
{
	public async Task AddAsync(
		AcademicYearEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AcademicYearEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AcademicYearEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AcademicYearEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AcademicYearEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AcademicYearEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
