using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Executes database writes for <see cref="ScholarshipEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ScholarshipCommand(IApplicationDbContext dbContext) : IScholarshipCommand
{
	public async Task AddAsync(
		ScholarshipEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ScholarshipEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ScholarshipEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ScholarshipEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ScholarshipEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ScholarshipEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
