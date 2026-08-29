using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Activities.Models;

namespace SmartSchool.Modules.Activities.Features.StudentActivity;

/// <summary>
/// Executes database writes for <see cref="StudentActivityEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentActivityCommand(IApplicationDbContext dbContext) : IStudentActivityCommand
{
	public async Task AddAsync(
		StudentActivityEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentActivityEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentActivityEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentActivityEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentActivityEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentActivityEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
