using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AIInquiry.Models;

namespace SmartSchool.Modules.AIInquiry.Persistence;

/// <summary>
/// Executes database writes for <see cref="LeadCaptureEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class LeadCaptureCommand(IApplicationDbContext dbContext) : ILeadCaptureCommand
{
	public async Task AddAsync(
		LeadCaptureEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<LeadCaptureEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		LeadCaptureEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<LeadCaptureEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		LeadCaptureEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<LeadCaptureEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
