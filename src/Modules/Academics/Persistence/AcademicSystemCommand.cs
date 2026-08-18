using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Academics.Models;

namespace SmartSchool.Modules.Academics.Persistence;

/// <summary>
/// Executes database writes for <see cref="AcademicSystemEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AcademicSystemCommand(IApplicationDbContext dbContext) : IAcademicSystemCommand
{
	public async Task AddAsync(
		AcademicSystemEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AcademicSystemEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AcademicSystemEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AcademicSystemEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AcademicSystemEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AcademicSystemEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
