using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Persistence;

/// <summary>
/// Executes database writes for <see cref="EmployeeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class EmployeeCommand(IApplicationDbContext dbContext) : IEmployeeCommand
{
	public async Task AddAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<EmployeeEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<EmployeeEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<EmployeeEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
