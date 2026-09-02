using SmartSchool.Modules.AITutor.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AITutor.Models;

namespace SmartSchool.Modules.AITutor.Features.StudentTopicMastery;

/// <summary>
/// Executes database writes for <see cref="StudentTopicMasteryEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentTopicMasteryCommand(IAITutorDbContext dbContext) : IStudentTopicMasteryCommand
{
	public async Task AddAsync(
		StudentTopicMasteryEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.StudentTopicMasteries
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentTopicMasteryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StudentTopicMasteries
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentTopicMasteryEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StudentTopicMasteries
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
