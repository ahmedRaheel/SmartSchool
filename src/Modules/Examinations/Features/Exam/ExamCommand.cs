using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.Exam;

/// <summary>
/// Executes database writes for <see cref="ExamEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ExamCommand(IApplicationDbContext dbContext) : IExamCommand
{
	public async Task AddAsync(
		ExamEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ExamEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ExamEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ExamEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ExamEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ExamEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
