using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Executes database writes for <see cref="KnowledgeDocumentEntity"/>.
/// The command owns persistence of its unit of work.
/// </summary>
public sealed class KnowledgeDocumentCommand(IApplicationDbContext dbContext) : IKnowledgeDocumentCommand
{
	public async Task AddAsync(
		KnowledgeDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		await dbContext
			.Set<KnowledgeDocumentEntity>()
			.AddAsync(entity, cancellationToken);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAsync(
		KnowledgeDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<KnowledgeDocumentEntity>()
			.Update(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAsync(
		KnowledgeDocumentEntity entity,
		CancellationToken cancellationToken)
	{
		dbContext
			.Set<KnowledgeDocumentEntity>()
			.Remove(entity);

		await dbContext.SaveChangesAsync(cancellationToken);
	}
}
