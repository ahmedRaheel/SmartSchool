using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Executes database writes for <see cref="AssignmentSubmissionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AssignmentSubmissionCommand(IApplicationDbContext dbContext) : IAssignmentSubmissionCommand
{
	public async Task AddAsync(
		AssignmentSubmissionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AssignmentSubmissionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AssignmentSubmissionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AssignmentSubmissionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AssignmentSubmissionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AssignmentSubmissionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
