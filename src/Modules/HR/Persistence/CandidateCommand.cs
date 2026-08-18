using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Executes database writes for <see cref="CandidateEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class CandidateCommand(IApplicationDbContext dbContext) : ICandidateCommand
{
	public async Task AddAsync(
		CandidateEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<CandidateEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		CandidateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CandidateEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		CandidateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<CandidateEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
