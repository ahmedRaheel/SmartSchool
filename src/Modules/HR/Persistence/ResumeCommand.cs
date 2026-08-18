using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Executes database writes for <see cref="ResumeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ResumeCommand(IApplicationDbContext dbContext) : IResumeCommand
{
	public async Task AddAsync(
		ResumeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ResumeEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ResumeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ResumeEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ResumeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ResumeEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
