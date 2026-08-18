using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Executes database writes for <see cref="PayslipEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PayslipCommand(IApplicationDbContext dbContext) : IPayslipCommand
{
	public async Task AddAsync(
		PayslipEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PayslipEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PayslipEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PayslipEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PayslipEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PayslipEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
