using SmartSchool.Modules.Students.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Features.StudentGuardian;

/// <summary>
/// Executes database writes for <see cref="StudentGuardianEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentGuardianCommand(IStudentsDbContext dbContext) : IStudentGuardianCommand
{
	public async Task AddAsync(
		StudentGuardianEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.StudentGuardians
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentGuardianEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StudentGuardians
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentGuardianEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.StudentGuardians
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
