using SmartSchool.Modules.Finance.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.FeeType;

/// <summary>
/// Executes database writes for <see cref="FeeTypeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class FeeTypeCommand(IFinanceDbContext dbContext) : IFeeTypeCommand
{
	public async Task AddAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.FeeTypes
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.FeeTypes
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.FeeTypes
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
