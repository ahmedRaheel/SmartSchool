using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Executes database writes for <see cref="KnowledgeChunkEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class KnowledgeChunkCommand(IApplicationDbContext dbContext) : IKnowledgeChunkCommand
{
	public async Task AddAsync(
		KnowledgeChunkEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<KnowledgeChunkEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		KnowledgeChunkEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<KnowledgeChunkEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		KnowledgeChunkEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<KnowledgeChunkEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
