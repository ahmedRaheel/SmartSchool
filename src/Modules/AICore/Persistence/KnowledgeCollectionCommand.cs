using SmartSchool.Modules.AICore.Models;

namespace SmartSchool.Modules.AICore.Persistence;

/// <summary>
/// Write-side persistence for KnowledgeCollection.
/// Transaction boundaries remain explicit in the application use case.
/// </summary>
public sealed class KnowledgeCollectionCommand : IKnowledgeCollectionCommand
{
    public Task AddAsync(
        KnowledgeCollection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeCollection create persistence has not been connected to the module DbContext.");
    }

    public Task UpdateAsync(
        KnowledgeCollection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeCollection update persistence has not been connected to the module DbContext.");
    }

    public Task DeleteAsync(
        KnowledgeCollection entity,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException(
            "KnowledgeCollection delete persistence has not been connected to the module DbContext.");
    }
}
