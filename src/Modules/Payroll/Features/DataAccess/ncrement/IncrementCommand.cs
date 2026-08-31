using SmartSchool.Modules.Payroll.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

using SmartSchool.Modules.Payroll.Features.Increment;

namespace SmartSchool.Modules.Payroll.Features.DataAccess.Increment;

/// <summary>
/// Executes database writes for <see cref="IncrementEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class IncrementCommand(IPayrollDbContext dbContext) : IIncrementCommand
{
	public async Task AddAsync(
		IncrementEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.Increments
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		IncrementEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Increments
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		IncrementEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.Increments
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
