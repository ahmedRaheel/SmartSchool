using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIParent.Models;

namespace SmartSchool.Modules.AIParent.Features.ParentConversation;

/// <summary>
/// Executes database writes for <see cref="ParentConversationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ParentConversationCommand(IApplicationDbContext dbContext) : IParentConversationCommand
{
	public async Task AddAsync(
		ParentConversationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ParentConversationEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ParentConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ParentConversationEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ParentConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ParentConversationEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
