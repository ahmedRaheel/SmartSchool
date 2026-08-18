using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Persistence;

/// <summary>
/// Executes database writes for <see cref="TutorMessageEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TutorMessageCommand(IApplicationDbContext dbContext) : ITutorMessageCommand
{
	public async Task AddAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<TutorMessageEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TutorMessageEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TutorMessageEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<TutorMessageEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
