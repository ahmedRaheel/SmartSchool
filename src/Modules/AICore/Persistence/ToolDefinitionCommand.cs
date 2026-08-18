using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Executes database writes for <see cref="ToolDefinitionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class ToolDefinitionCommand(IApplicationDbContext dbContext) : IToolDefinitionCommand
{
	public async Task AddAsync(
		ToolDefinitionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<ToolDefinitionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		ToolDefinitionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ToolDefinitionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		ToolDefinitionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<ToolDefinitionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
