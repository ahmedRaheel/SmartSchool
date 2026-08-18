using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Transport.Models;

namespace SmartSchool.Modules.Transport.Persistence;

/// <summary>
/// Executes database writes for <see cref="StudentTransportEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class StudentTransportCommand(IApplicationDbContext dbContext) : IStudentTransportCommand
{
	public async Task AddAsync(
		StudentTransportEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<StudentTransportEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		StudentTransportEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentTransportEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		StudentTransportEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<StudentTransportEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
