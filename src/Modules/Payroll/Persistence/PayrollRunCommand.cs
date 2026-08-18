using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Executes database writes for <see cref="PayrollRunEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PayrollRunCommand(IApplicationDbContext dbContext) : IPayrollRunCommand
{
	public async Task AddAsync(
		PayrollRunEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PayrollRunEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PayrollRunEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PayrollRunEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PayrollRunEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PayrollRunEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
