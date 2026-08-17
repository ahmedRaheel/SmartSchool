using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for KnowledgeCollectionEntity.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class KnowledgeCollectionCommand : IKnowledgeCollectionCommand
{
    public Task AddAsync(
        KnowledgeCollectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeCollectionEntity create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        KnowledgeCollectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeCollectionEntity update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        KnowledgeCollectionEntity entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeCollectionEntity delete persistence has not been connected to the module DbContext.");
    }
}
