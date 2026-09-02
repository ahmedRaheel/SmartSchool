using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.TutorSession;

/// <summary>
/// Executes database writes for <see cref="TutorSessionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class TutorSessionCommand(IAITutorDbContext dbContext) : ITutorSessionCommand
{
	public async Task AddAsync(
		TutorSessionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.TutorSessions
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		TutorSessionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TutorSessions
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		TutorSessionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.TutorSessions
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
