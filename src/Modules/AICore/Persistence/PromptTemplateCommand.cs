using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Executes database writes for <see cref="PromptTemplateEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class PromptTemplateCommand(IApplicationDbContext dbContext) : IPromptTemplateCommand
{
	public async Task AddAsync(
		PromptTemplateEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<PromptTemplateEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		PromptTemplateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PromptTemplateEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		PromptTemplateEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<PromptTemplateEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
