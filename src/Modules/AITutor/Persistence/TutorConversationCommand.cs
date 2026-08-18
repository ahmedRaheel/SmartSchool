using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Executes database writes for <see cref="TutorConversationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TutorConversationCommand(IApplicationDbContext dbContext) : ITutorConversationCommand
{
	public async Task AddAsync(
		TutorConversationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<TutorConversationEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TutorConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TutorConversationEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TutorConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TutorConversationEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
