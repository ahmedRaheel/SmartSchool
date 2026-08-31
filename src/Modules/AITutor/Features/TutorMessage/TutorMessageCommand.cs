using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.TutorMessage;

/// <summary>
/// Executes database writes for <see cref="TutorMessageEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TutorMessageCommand(IAITutorDbContext dbContext) : ITutorMessageCommand
{
	public async Task AddAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.TutorMessages
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TutorMessages
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TutorMessages
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
