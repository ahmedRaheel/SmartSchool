using SmartSchool.Modules.Payroll.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Features.PayrollRun;

/// <summary>
/// Executes database writes for <see cref="PayrollRunEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PayrollRunCommand(IPayrollDbContext dbContext) : IPayrollRunCommand
{
	public async Task AddAsync(
		PayrollRunEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.PayrollRuns
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PayrollRunEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.PayrollRuns
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PayrollRunEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.PayrollRuns
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
