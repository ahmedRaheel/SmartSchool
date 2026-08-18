using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIPrediction.Models;

namespace SmartSchool.Modules.AIPrediction.Persistence;

/// <summary>
/// Executes database writes for <see cref="StudentInterventionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentInterventionCommand(IApplicationDbContext dbContext) : IStudentInterventionCommand
{
	public async Task AddAsync(
		StudentInterventionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentInterventionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentInterventionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentInterventionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentInterventionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentInterventionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
