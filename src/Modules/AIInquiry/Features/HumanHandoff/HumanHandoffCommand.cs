using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Features.HumanHandoff;

/// <summary>
/// Executes database writes for <see cref="HumanHandoffEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class HumanHandoffCommand(IApplicationDbContext dbContext) : IHumanHandoffCommand
{
	public async Task AddAsync(
		HumanHandoffEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<HumanHandoffEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		HumanHandoffEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<HumanHandoffEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		HumanHandoffEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<HumanHandoffEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
