using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Persistence;

/// <summary>
/// Executes database writes for <see cref="FeeTypeEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class FeeTypeCommand(IApplicationDbContext dbContext) : IFeeTypeCommand
{
	public async Task AddAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<FeeTypeEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<FeeTypeEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		FeeTypeEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<FeeTypeEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
