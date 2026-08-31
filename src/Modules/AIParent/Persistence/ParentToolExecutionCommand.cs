using SmartSchool.Modules.AIParent.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Persistence;

/// <summary>
/// Executes database writes for <see cref="ParentToolExecutionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ParentToolExecutionCommand(IAIParentDbContext dbContext) : IParentToolExecutionCommand
{
	public async Task AddAsync(
		ParentToolExecutionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.ParentToolExecutions
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ParentToolExecutionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ParentToolExecutions
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ParentToolExecutionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ParentToolExecutions
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
