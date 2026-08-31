using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.TutorConversation;

/// <summary>
/// Executes database writes for <see cref="TutorConversationEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TutorConversationCommand(IAITutorDbContext dbContext) : ITutorConversationCommand
{
	public async Task AddAsync(
		TutorConversationEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.TutorConversations
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TutorConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TutorConversations
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TutorConversationEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TutorConversations
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
