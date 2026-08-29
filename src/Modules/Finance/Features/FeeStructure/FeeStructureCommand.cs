using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Finance.Models;

namespace SmartSchool.Modules.Finance.Features.FeeStructure;

/// <summary>
/// Executes database writes for <see cref="FeeStructureEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class FeeStructureCommand(IApplicationDbContext dbContext) : IFeeStructureCommand
{
	public async Task AddAsync(
		FeeStructureEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<FeeStructureEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		FeeStructureEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<FeeStructureEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		FeeStructureEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<FeeStructureEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
