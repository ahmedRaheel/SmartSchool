using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Examinations.Models;

namespace SmartSchool.Modules.Examinations.Features.StudentExamResult;

/// <summary>
/// Executes database writes for <see cref="StudentExamResultEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentExamResultCommand(IApplicationDbContext dbContext) : IStudentExamResultCommand
{
	public async Task AddAsync(
		StudentExamResultEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentExamResultEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentExamResultEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentExamResultEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentExamResultEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentExamResultEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
