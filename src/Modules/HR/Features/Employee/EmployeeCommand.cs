using SmartSchool.Modules.HR.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.HR.Models;

namespace SmartSchool.Modules.HR.Features.Employee;

/// <summary>
/// Executes database writes for <see cref="EmployeeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class EmployeeCommand(IHRDbContext dbContext) : IEmployeeCommand
{
	public async Task AddAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Employees
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Employees
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		EmployeeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Employees
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
