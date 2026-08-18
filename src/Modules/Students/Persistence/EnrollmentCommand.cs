using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Students.Models;

namespace SmartSchool.Modules.Students.Persistence;

/// <summary>
/// Executes database writes for <see cref="EnrollmentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class EnrollmentCommand(IApplicationDbContext dbContext) : IEnrollmentCommand
{
	public async Task AddAsync(
		EnrollmentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<EnrollmentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		EnrollmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<EnrollmentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		EnrollmentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<EnrollmentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
