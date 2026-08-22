using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Executes database writes for <see cref="KnowledgeCollectionEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class KnowledgeCollectionCommand(IApplicationDbContext dbContext) : IKnowledgeCollectionCommand
{
	public async Task AddAsync(
		KnowledgeCollectionEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<KnowledgeCollectionEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		KnowledgeCollectionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<KnowledgeCollectionEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		KnowledgeCollectionEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<KnowledgeCollectionEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
