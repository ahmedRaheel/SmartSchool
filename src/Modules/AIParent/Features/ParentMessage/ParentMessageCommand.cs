using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Features.ParentMessage;

/// <summary>
/// Executes database writes for <see cref="ParentMessageEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ParentMessageCommand(IApplicationDbContext dbContext) : IParentMessageCommand
{
	public async Task AddAsync(
		ParentMessageEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ParentMessageEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ParentMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ParentMessageEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ParentMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ParentMessageEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
