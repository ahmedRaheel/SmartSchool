using SmartSchool.Modules.AIInquiry.Persistence;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Executes database writes for <see cref="HumanHandoffEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class HumanHandoffCommand(IAIInquiryDbContext dbContext) : IHumanHandoffCommand
{
	public async Task AddAsync(
		HumanHandoffEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext.HumanHandoffs
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		HumanHandoffEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.HumanHandoffs
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		HumanHandoffEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext.HumanHandoffs
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
