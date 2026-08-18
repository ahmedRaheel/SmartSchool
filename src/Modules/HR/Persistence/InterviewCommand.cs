using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Executes database writes for <see cref="InterviewEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class InterviewCommand(IApplicationDbContext dbContext) : IInterviewCommand
{
	public async Task AddAsync(
		InterviewEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<InterviewEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		InterviewEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InterviewEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		InterviewEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<InterviewEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
