using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Payroll.Models;

namespace SmartSchool.Modules.Payroll.Persistence;

/// <summary>
/// Executes database writes for <see cref="SalaryStructureEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class SalaryStructureCommand(IApplicationDbContext dbContext) : ISalaryStructureCommand
{
	public async Task AddAsync(
		SalaryStructureEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<SalaryStructureEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		SalaryStructureEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<SalaryStructureEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		SalaryStructureEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<SalaryStructureEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
