using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Communication.Models;

namespace SmartSchool.Modules.Communication.Persistence;

/// <summary>
/// Executes database writes for <see cref="ConversationParticipantEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ConversationParticipantCommand(IApplicationDbContext dbContext) : IConversationParticipantCommand
{
	public async Task AddAsync(
		ConversationParticipantEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ConversationParticipantEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ConversationParticipantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ConversationParticipantEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ConversationParticipantEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ConversationParticipantEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
