using SmartSchool.Modules.Communication.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Features.Conversation;

/// <summary>
/// Executes database writes for <see cref="ConversationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ConversationCommand(ICommunicationDbContext dbContext) : IConversationCommand
{
	public async Task AddAsync(
		ConversationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Conversations
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Conversations
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Conversations
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
