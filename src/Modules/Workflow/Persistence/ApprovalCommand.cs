using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.Workflow.Models;

namespace SmartSchool.Modules.Workflow.Persistence;

/// <summary>
/// Executes database writes for <see cref="ApprovalEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ApprovalCommand(IApplicationDbContext dbContext) : IApprovalCommand
{
	public async Task AddAsync(
		ApprovalEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ApprovalEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ApprovalEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ApprovalEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ApprovalEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ApprovalEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
