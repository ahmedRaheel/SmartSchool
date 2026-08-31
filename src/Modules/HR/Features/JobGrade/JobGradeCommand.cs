using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.JobGrade;

/// <summary>
/// Executes database writes for <see cref="JobGradeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class JobGradeCommand(IHRDbContext dbContext) : IJobGradeCommand
{
	public async Task AddAsync(
		JobGradeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.JobGrades
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		JobGradeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.JobGrades
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		JobGradeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.JobGrades
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
