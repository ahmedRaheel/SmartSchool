using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Learning.Models;

namespace SmartSchool.Modules.Learning.Persistence;

/// <summary>
/// Executes database writes for <see cref="AssignmentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AssignmentCommand(IApplicationDbContext dbContext) : IAssignmentCommand
{
	public async Task AddAsync(
		AssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AssignmentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AssignmentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AssignmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AssignmentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
