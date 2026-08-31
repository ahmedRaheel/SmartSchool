using SmartSchool.Modules.Students.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.Student;

/// <summary>
/// Executes database writes for <see cref="StudentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentCommand(IStudentsDbContext dbContext) : IStudentCommand
{
	public async Task AddAsync(
		StudentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Students
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Students
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Students
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
