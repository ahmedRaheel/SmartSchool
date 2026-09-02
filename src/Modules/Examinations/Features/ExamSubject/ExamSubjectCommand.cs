using SmartSchool.Modules.Examinations.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.ExamSubject;

/// <summary>
/// Executes database writes for <see cref="ExamSubjectEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ExamSubjectCommand(IExaminationsDbContext dbContext) : IExamSubjectCommand
{
	public async Task AddAsync(
		ExamSubjectEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.ExamSubjects
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ExamSubjectEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ExamSubjects
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ExamSubjectEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.ExamSubjects
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
