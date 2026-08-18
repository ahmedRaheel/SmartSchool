using SmartSchool.Application.Persistence;
using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// EF-backed write persistence for KnowledgeCollectionEntity.
/// </summary>
public sealed class KnowledgeCollectionCommand(IEfMockStore store) : IKnowledgeCollectionCommand
{
	public Task AddAsync(KnowledgeCollectionEntity entity, CancellationToken cancellationToken)
	{
		return store.AddAsync(entity, cancellationToken);
	}

	public Task UpdateAsync(KnowledgeCollectionEntity entity, CancellationToken cancellationToken)
	{
		return store.UpdateAsync(entity, cancellationToken);
	}

	public Task DeleteAsync(KnowledgeCollectionEntity entity, CancellationToken cancellationToken)
	{
		return store.DeleteAsync(entity, cancellationToken);
	}

}
