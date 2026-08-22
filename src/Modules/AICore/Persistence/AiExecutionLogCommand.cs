using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Executes database writes for <see cref="AiExecutionLogEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class AiExecutionLogCommand(IApplicationDbContext dbContext) : IAiExecutionLogCommand
{
	public async Task AddAsync(
		AiExecutionLogEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<AiExecutionLogEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		AiExecutionLogEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AiExecutionLogEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		AiExecutionLogEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<AiExecutionLogEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
