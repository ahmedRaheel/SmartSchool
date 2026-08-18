using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Admissions.Models;

namespace SmartSchool.Modules.Admissions.Persistence;

/// <summary>
/// Executes database writes for <see cref="ApplicantEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ApplicantCommand(IApplicationDbContext dbContext) : IApplicantCommand
{
	public async Task AddAsync(
		ApplicantEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ApplicantEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ApplicantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ApplicantEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ApplicantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ApplicantEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
